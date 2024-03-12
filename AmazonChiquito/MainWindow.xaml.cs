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
        public List<Usuario> usuarios;
        public List<Producto> productosDesdeAPI;
        private TextBox usernameBox;
        private PasswordBox passwordBox;
        private TextBlock textBlock;

        public MainWindow()
        {
            InitializeComponent();

            // Lista con los productos recibidos de la API
            productosDesdeAPI = ObtenerProductosDesdeAPI();

            usuarios = ObtenerUsuariosDesdeAPI();

            foreach (Usuario u in usuarios)
            {
                System.Diagnostics.Debug.WriteLine("Nombre: "+u.Username);
            }

            // Muestra los productos en la vista
            MostrarProductos(productosDesdeAPI);
        }

        private List<Usuario> ObtenerUsuariosDesdeAPI()
        {
            List<Usuario> usuarios = new List<Usuario>();

            using (HttpClient client = new HttpClient())
            {
                var response = client.GetAsync("http://localhost:3000/usuarios").Result;
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var resultado = response.Content.ReadAsStringAsync().Result;
                    var u = JsonConvert.DeserializeObject<List<Usuario>>(resultado);
                    foreach (Usuario p in u) usuarios.Add(p);
                }
                else
                {
                    throw new Exception("Error al cargar la información de la API.");
                }
            }

            return usuarios;
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

        private void onHolaIdentificate(object sender, MouseEventArgs e)
        {
            // Acceder al Grid
            Grid productosGrid = (Grid)FindName("productosGrid");
            productosGrid.Children.Clear();


            productosGrid.VerticalAlignment = VerticalAlignment.Center;
            productosGrid.HorizontalAlignment = HorizontalAlignment.Center;

            // Crear el elemento Border
            Border newBorder = new Border();
            newBorder.BorderBrush = Brushes.DarkGray;
            newBorder.BorderThickness = new Thickness(2);
            newBorder.CornerRadius = new CornerRadius(5);
            newBorder.Padding = new Thickness(20);
            newBorder.Height = 256;
            newBorder.Margin = new Thickness(0, 0, 0, 0);

            // Crear el Grid dentro del Border
            Grid newGrid = new Grid();

            // Definir las filas del nuevo Grid
            newGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            newGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            newGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            newGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            newGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            newGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto }); // Nueva fila para el botón

            // TextBlock "Inicio de Sesión"
            textBlock = new TextBlock();
            textBlock.Name = "textBlock";
            textBlock.Text = "Inicio de Sesión";
            textBlock.FontSize = 20;
            textBlock.FontWeight = FontWeights.Bold;
            textBlock.Margin = new Thickness(0, 0, 0, 20);
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            textBlock.TextWrapping = TextWrapping.NoWrap;
            textBlock.Height = 20;
            Grid.SetRow(textBlock, 0);

            // Etiqueta y TextBox para el nombre de usuario
            TextBlock usuarioLabel = new TextBlock();
            usuarioLabel.Text = "Nombre de Usuario:";
            usuarioLabel.Width = 150;
            usuarioLabel.Height = 60;
            Grid.SetRow(usuarioLabel, 1);

            usernameBox = new TextBox();
            usernameBox.Name = "usernameBox";
            usernameBox.Width = 151;
            usernameBox.Height = 20;
            Grid.SetRow(usernameBox, 1);

            // Etiqueta y PasswordBox para la contraseña
            TextBlock passwordLabel = new TextBlock();
            passwordLabel.Text = "Contraseña:";
            passwordLabel.Width = 149;
            passwordLabel.Height = 60;
            Grid.SetRow(passwordLabel, 2);

            passwordBox = new PasswordBox();
            passwordBox.Name = "passwordBox";
            passwordBox.Width = 152;
            passwordBox.Height = 20;
            Grid.SetRow(passwordBox, 2);

            // CheckBox para aceptar condiciones
            CheckBox checkBox = new CheckBox();
            checkBox.Content = "Acepto las condiciones";
            checkBox.Margin = new Thickness(0, 0, 0, 10);
            checkBox.HorizontalAlignment = HorizontalAlignment.Center;
            checkBox.Height = 15;
            Grid.SetRow(checkBox, 4);

            // Botón de Iniciar Sesión
            Button button = new Button();
            button.Content = "Iniciar Sesión";
            button.Width = 147;
            button.Height = 20;
            button.HorizontalAlignment = HorizontalAlignment.Center;
            button.Background = Brushes.DodgerBlue;
            button.Foreground = Brushes.White;
            button.Click += login;
            Grid.SetRow(button, 5);

            // Agregar elementos al nuevo Grid
            newGrid.Children.Add(textBlock);
            newGrid.Children.Add(usuarioLabel);
            newGrid.Children.Add(usernameBox);
            newGrid.Children.Add(passwordLabel);
            newGrid.Children.Add(passwordBox);
            newGrid.Children.Add(checkBox);
            newGrid.Children.Add(button);

            // Agregar el nuevo Grid al Border
            newBorder.Child = newGrid;

            // Agregar el Border al Grid productosGrid
            Grid.SetColumn(newBorder, 1);
            productosGrid.Children.Add(newBorder);
        }

        private void onLogoClick(object sender, RoutedEventArgs e)
        {
            MostrarProductos(productosDesdeAPI);
        }

        private void onCategory(object sender, RoutedEventArgs e)
        {
            Grid productosGrid = (Grid)FindName("productosGrid");
            productosGrid.Margin = new Thickness(0, 90, 0, 0);

            // Crear el TextBlock
            TextBlock categoryTextBlock = new TextBlock();
            categoryTextBlock.Text = "Sección de informática";
            categoryTextBlock.Foreground = Brushes.White;
            categoryTextBlock.Background = Brushes.DodgerBlue;
            categoryTextBlock.VerticalAlignment = VerticalAlignment.Top;
            categoryTextBlock.HorizontalAlignment = HorizontalAlignment.Right;
            categoryTextBlock.TextAlignment = TextAlignment.Center;
            categoryTextBlock.Margin = new Thickness(0, 0, 0, 0);
            categoryTextBlock.Height = 80;
            categoryTextBlock.Width = 1060;
            categoryTextBlock.MouseLeftButtonUp += onCloseCategory;
            categoryTextBlock.FontSize = 44; // Tamaño de fuente 24
            categoryTextBlock.FontWeight = FontWeights.Bold; // Texto en negrita


            // Agregar el TextBlock al Grid
            Grid.SetColumn(categoryTextBlock, 0);
            Grid.SetColumnSpan(categoryTextBlock, 4);
            Grid.SetRow(categoryTextBlock, 0);
            mainGrid.Children.Add(categoryTextBlock);

            // Crear el TextBlock
            TextBlock categoryTextBlock2 = new TextBlock();
            categoryTextBlock2.Foreground = Brushes.White;
            categoryTextBlock2.Background = Brushes.DodgerBlue;
            categoryTextBlock2.VerticalAlignment = VerticalAlignment.Top;
            categoryTextBlock2.HorizontalAlignment = HorizontalAlignment.Left;
            categoryTextBlock2.TextAlignment = TextAlignment.Center;
            categoryTextBlock2.Margin = new Thickness(0, 0, 0, 0);
            categoryTextBlock2.Height = 700;
            categoryTextBlock2.Width = 226;
            categoryTextBlock2.MouseLeftButtonUp += onCloseCategory;
            categoryTextBlock2.FontSize = 44; // Tamaño de fuente 24
            categoryTextBlock2.FontWeight = FontWeights.Bold; // Texto en negrita

            Image imagenCategoria = new Image();
            imagenCategoria.Source = new BitmapImage(new Uri("/Img/informatica.png", UriKind.RelativeOrAbsolute));
            imagenCategoria.Width = 160;
            imagenCategoria.Height = 160;
            imagenCategoria.Margin = new Thickness(30, 30, 0, 0);
            imagenCategoria.HorizontalAlignment = HorizontalAlignment.Left;
            imagenCategoria.VerticalAlignment = VerticalAlignment.Top;

            Grid.SetColumn(imagenCategoria, 0);
            // Agregar el TextBlock al Grid
            Grid.SetColumn(categoryTextBlock2, 1);
            Grid.SetRowSpan(categoryTextBlock2, 4);
            Grid.SetRow(categoryTextBlock2, 1);
            mainGrid.Children.Add(categoryTextBlock2);
            mainGrid.Children.Add(imagenCategoria);

        }

        private void onCloseCategory(object sender, MouseButtonEventArgs e)
        {
            // Remover el TextBlock al hacer clic
            mainGrid.Children.Remove((UIElement)sender);
        }

        private void login(object sender, RoutedEventArgs e)

        {
            Console.WriteLine("Entra");
            // Obtener el nombre de usuario y la contraseña ingresados
            string username = usernameBox.Text;
            string password = passwordBox.Password;
            bool correct = false;

            // Iterar sobre la lista de usuarios para verificar el inicio de sesión
            foreach (Usuario usuario in usuarios)
            {
                Console.WriteLine("Input: " + username + " / usuario nick: " + usuario.Username);
                if (usuario.Username == username && usuario.Password == password)
                {
                    correct = true;
                    break;
                }
            }

            if (!correct)
            {
                textBlock.Text = "Datos incorrectos";
            } else
            {
                TextBlock loginText = (TextBlock)FindName("loginText");
                loginText.Text = "Hola, "+username;
                MostrarProductos(productosDesdeAPI);
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
