using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
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
            return new List<Producto>
            {
                new Producto(1, "Laptop", "Potente computadora portátil", 1999.99f, true, Categoria.Informatica, ".\\Img\\amazon-logo.png"),
                new Producto(2, "Camiseta", "Camiseta de algodón de alta calidad", 19.99f, false, Categoria.Moda, ".\\Img\\amazon-logo.png"),
                new Producto(3, "Libro", "Bestseller del año", 29.99f, true, Categoria.Libros, ".\\Img\\amazon-logo.png"),
                new Producto(4, "Sartén", "Sartén antiadherente", 39.99f, false, Categoria.HogarYCocina, ".\\Img\\amazon-logo.png"),
                new Producto(5, "Zapatos", "Zapatos elegantes", 69.99f, true, Categoria.Moda, ".\\Img\\amazon-logo.png"),
                new Producto(6, "Teclado", "Teclado mecánico para gamers", 89.99f, true, Categoria.Informatica, ".\\Img\\amazon-logo.png"),
                new Producto(7, "Taza", "Taza con diseño divertido", 9.99f, false, Categoria.HogarYCocina, ".\\Img\\amazon-logo.png"),
                new Producto(8, "Mochila", "Mochila resistente al agua", 49.99f, true, Categoria.Moda, ".\\Img\\amazon-logo.png"),
                new Producto(9, "Monitor", "Monitor de alta resolución", 299.99f, true, Categoria.Informatica, ".\\Img\\amazon-logo.png"),
                new Producto(10, "Cocina eléctrica", "Cocina eléctrica multifunción", 129.99f, false, Categoria.HogarYCocina, ".\\Img\\amazon-logo.png"),
                new Producto(11, "Reloj", "Reloj de pulsera elegante", 149.99f, true, Categoria.Moda, ".\\Img\\amazon-logo.png"),
                new Producto(12, "Altavoces", "Altavoces Bluetooth de alta calidad", 79.99f, true, Categoria.Informatica, ".\\Img\\amazon-logo.png"),
                new Producto(13, "Cojines", "Cojines decorativos para el hogar", 14.99f, false, Categoria.HogarYCocina, ".\\Img\\amazon-logo.png"),
                new Producto(14, "Gafas de sol", "Gafas de sol modernas", 59.99f, true, Categoria.Moda, ".\\Img\\amazon-logo.png")
            };
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
                CambiarColorFondo(border, Colors.LightGray);
            }
        }

        // Evento cuando el raton sale de la vista del producto
        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Border border)
            {
                CambiarColorFondo(border, (Color)ColorConverter.ConvertFromString("#F5F5F5"));
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
            imagenProducto.Width = 200;
            imagenProducto.Height = 200;
            imagenProducto.Margin = new Thickness(10, 0, 0, 56);
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

            return bordeProducto;
        }
    }
}
