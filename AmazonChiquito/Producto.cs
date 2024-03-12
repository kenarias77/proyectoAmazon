using AmazonChiquito;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

[Serializable]
public class Producto
{
    private int id;
    private string nombre;
    private string descripcion;
    private string imagen;
    private float precio;
    private bool favorito;
    private Categoria categoria;

    // Constructor por defecto
    public Producto(int id, string nombre, string descripcion, float precio, bool favorito, Categoria categoria, string imagen)
    {
        Id = id;
        Nombre = nombre;
        Descripcion = descripcion;
        Precio = precio;
        Favorito = favorito;
        Categoria = categoria;
        Imagen = imagen;
    }

    // Funcion para obtener todos los productos de una api
    public static async Task<List<Producto>> ObtenerProductosDesdeAPI()
    {
        List<Producto> productos = new List<Producto>();

        try
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("http://localhost:3000/amazon/productos");

                if (response.IsSuccessStatusCode)
                {
                    productos = await response.Content.ReadAsAsync<List<Producto>>();
                }
                else
                {
                    Console.WriteLine("Error al obtener productos desde la API. Código de estado: " + response.StatusCode);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error al obtener productos desde la API: " + e.Message);
        }

        return productos;
    }

    // Getter y Setter para el atributo 'id'
    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    // Getter y Setter para el atributo 'nombre'
    public string Nombre
    {
        get { return nombre; }
        set { nombre = value; }
    }

    // Getter y Setter para el atributo 'imagen'
    public string Imagen
    {
        get { return imagen; }
        set { imagen = value; }
    }

    // Getter y Setter para el atributo 'descripcion'
    public string Descripcion
    {
        get { return descripcion; }
        set { descripcion = value; }
    }

    // Getter y Setter para el atributo 'precio'
    public float Precio
    {
        get { return precio; }
        set { if (value > 0) precio = value; }
    }

    // Getter y Setter para el atributo 'favorito'
    public bool Favorito
    {
        get { return favorito; }
        set { favorito = value; }
    }

    // Getter y Setter para el atributo 'categoria'
    public Categoria Categoria
    {
        get { return categoria; }
        set { categoria = value; }
    }
}
