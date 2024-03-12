using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AmazonChiquito
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Lista con los productos recibidos de la API
            List<Producto> productosDesdeAPI = ObtenerProductosDesdeAPI();

            // Muestra los productos en la vista
            MostrarProductos(productosDesdeAPI);
        }

        // Función de ejemplo para obtener productos (simulada)
        private List<Producto> ObtenerProductosDesdeAPI()
        {
            List<Producto> listaProductos = new List<Producto>();

            using (HttpClient client = new HttpClient())
            {
                var response = client.GetAsync("http://localhost:3000/productos").Result;
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var resultado = response.Content.ReadAsStringAsync().Result;
                    listaProductos = JsonConvert.DeserializeObject<List<Producto>>(resultado);
                }
                else
                {
                    throw new Exception("Error al cargar la información de la API.");
                }
            }
            return listaProductos;
        }

        private void VaciarProductos()
        {
            Grid productosGrid = (Grid)FindName("productosGrid");
            productosGrid.Children.Clear();
        }

        private void MostrarProductos(List<Producto> productos)
        {
            // Obtiene el componente XAML
            Grid productosGrid = (Grid)FindName("productosGrid");

            // Recorre la lista de productos y agrega los productos al componente XAML
            foreach (Producto producto in productos)
            {
                // Crea un contenedor para el producto
                Grid productoGrid = new Grid();

                // Asgina formato al contenedor del producto
                productoGrid.VerticalAlignment = VerticalAlignment.Top;

                // Agrega la imagen al contenedor
                productoGrid.Children.Add(recursoImagenProducto(producto));

                // Agrega el nombre del producto al contenedor
                productoGrid.Children.Add(recursoNombreProducto(producto));

                // Agrega el precio del producto al contenedor
                productoGrid.Children.Add(recursoPrecioProducto(producto));

                // Determina la posición del producto en el Grid
                int columna = productosGrid.Children.Count % 4;
                int fila = productosGrid.Children.Count / 4;

                // Crea las filas necesarias
                annadirFilas(productosGrid);

                // Agrega el contenedor del producto al Grid
                Border bordeProducto = vistaProducto(productoGrid);
                productosGrid.Children.Add(bordeProducto);

                // Establece la posición del producto en el Grid
                Grid.SetColumn(bordeProducto, columna);
                Grid.SetRow(bordeProducto, fila);
            }
        }

        // Evento cuando el raton entra en la vista del producto
        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Border border)
            {
                CambiarColorFondo(border, Colors.LightBlue);
            }
        }

        // Evento cuando el raton sale de la vista del producto
        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Border border)
            {
                CambiarColorFondo(border, Colors.DodgerBlue);
            }
        }

        // Cambia el color del componente Border que contiene el productGrid
        private void CambiarColorFondo(Border border, Color color)
        {
            border.Background = new SolidColorBrush(color);
        }

        // Crea una fila al principio y cada 4 productos
        private void annadirFilas(Grid productosGrid)
        {
            if(productosGrid.Children.Count == 0 || productosGrid.Children.Count % 4 == 0)
            {
                RowDefinition nuevaFila = new RowDefinition();
                nuevaFila.Height = new GridLength(256); // Establecer la altura de la fila
                productosGrid.RowDefinitions.Add(nuevaFila);
            }
        }

        // Crea el recurso Image del producto
        private Image recursoImagenProducto(Producto producto)
        {
            Image imagenProducto = new Image();
            imagenProducto.Source = new BitmapImage(new Uri(producto.Imagen, UriKind.RelativeOrAbsolute));
            imagenProducto.Width = 150;
            imagenProducto.Height = 200;
            imagenProducto.Margin = new Thickness(25, 0, 0, 65);
            imagenProducto.HorizontalAlignment = HorizontalAlignment.Left;

            return imagenProducto;
        }

        // Crea el recurso TextBlock con el nombre del producto
        private TextBlock recursoNombreProducto(Producto producto)
        {
            TextBlock nombreProducto = new TextBlock();
            nombreProducto.Text = producto.Nombre;
            nombreProducto.Margin = new Thickness(10, 186, 48, 44);
            nombreProducto.FontSize = 16;
            nombreProducto.FontWeight = FontWeights.Bold;

            return nombreProducto;
        }

        // Crea el recurso TextBlock con el precio del producto
        private TextBlock recursoPrecioProducto(Producto producto)
        {
            TextBlock precioProducto = new TextBlock();
            precioProducto.Text = producto.Precio.ToString("C");
            precioProducto.Margin = new Thickness(10, 217, 48, 13);
            precioProducto.FontSize = 24;
            precioProducto.FontWeight = FontWeights.Bold;

            return precioProducto;
        }

        // Crea un borde para el producto y le asigna eventos
        private Border vistaProducto(Grid productoGrid)
        {
            // Crear un Border y añadir el Grid como su contenido
            Border bordeProducto = new Border();
            bordeProducto.Child = productoGrid;

            // Establecer las esquinas redondeadas del Border
            bordeProducto.CornerRadius = new CornerRadius(10);
            bordeProducto.BorderBrush = Brushes.White;
            bordeProducto.BorderThickness = new Thickness(2);
            CambiarColorFondo(bordeProducto, (Color)ColorConverter.ConvertFromString("#F5F5F5"));

            // Asigna eventos MouseEnter y MouseLeave al producto
            bordeProducto.MouseEnter += OnMouseEnter;
            bordeProducto.MouseLeave += OnMouseLeave;
            bordeProducto.Background = Brushes.DodgerBlue;

            return bordeProducto;
        }

        //FUNCIÓN BUSCADOR
        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            List<Producto> filtroProductos = new List<Producto>();
            using (HttpClient client = new HttpClient())
            {
                var ruta = new Uri("http://localhost:3000/buscador");
                var buscador = new Buscador()
                {
                    texto = searchBox.Text
                };
                var buscadorJSON = JsonConvert.SerializeObject(buscador);
                var contenido = new StringContent(buscadorJSON, Encoding.UTF8, "application/json");
                var resultado = client.PostAsync(ruta, contenido).Result.Content.ReadAsStringAsync().Result;
                filtroProductos = JsonConvert.DeserializeObject<List<Producto>>(resultado);
            }
            VaciarProductos();
            MostrarProductos(filtroProductos);
        }

        //FUNCIÓN FAVORITOS
        private void Favoritos_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            List<Producto> productosFavoritos = new List<Producto>();

            using (HttpClient client = new HttpClient())
            {
                var response = client.GetAsync("http://localhost:3000/favoritos").Result;
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var resultado = response.Content.ReadAsStringAsync().Result;
                    productosFavoritos = JsonConvert.DeserializeObject<List<Producto>>(resultado);
                }
                else
                {
                    throw new Exception("Error al cargar la información de la API.");
                }
            }
            VaciarProductos();
            MostrarProductos(productosFavoritos);
        }

        //FUNCIÓN RECOMENDADOS
        private void Recomendados_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            List<Producto> productosRecomendados = new List<Producto>();

            using (HttpClient client = new HttpClient())
            {
                var response = client.GetAsync("http://localhost:3000/recomendados").Result;
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var resultado = response.Content.ReadAsStringAsync().Result;
                    List<Producto> productos = JsonConvert.DeserializeObject<List<Producto>>(resultado);
                    Random random = new Random();
                    var length = productos.Count;
                    for (int i=0; i < length; i++)
                    {
                        int randomNumber = random.Next(productos.Count);
                        productosRecomendados.Add(productos[randomNumber]);
                        productos.RemoveAt(randomNumber);
                    }
                }
                else
                {
                    throw new Exception("Error al cargar la información de la API.");
                }
            }
            VaciarProductos();
            MostrarProductos(productosRecomendados);
        }

        //FUNNCIÓN QUE UTILIZAN LAS 4 CATEGORÍAS PARA FILTRAR SUS PRODUCTOS
        private void Filtrador_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;

            if (textBlock != null)
            {
                string parametro = textBlock.Tag as string;

                List<Producto> filtroProductos = new List<Producto>();
                using (HttpClient client = new HttpClient())
                {
                    var ruta = new Uri("http://localhost:3000/filtrador");
                    var filtrador = new Filtrador()
                    {
                        filtro = parametro
                    };
                    var filtradorJSON = JsonConvert.SerializeObject(filtrador);
                    var contenido = new StringContent(filtradorJSON, Encoding.UTF8, "application/json");
                    var resultado = client.PostAsync(ruta, contenido).Result.Content.ReadAsStringAsync().Result;
                    filtroProductos = JsonConvert.DeserializeObject<List<Producto>>(resultado);
                }
                VaciarProductos();
                MostrarProductos(filtroProductos);
            }
        }
    }
}
