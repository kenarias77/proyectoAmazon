const express = require('express');
const path = require('path');
const bodyParser = require('body-parser');
const fs = require('fs');

const app = express();
const static_path = path.join(__dirname, 'public');

app.use(bodyParser.json());
app.use(express.static(static_path));
app.use(bodyParser.urlencoded({extended: true}));

app.get('/productos', (req, res) =>{
    res.setHeader('Content-type','text/json');
    const file = fs.readFileSync('./productos.json','UTF-8');
    res.send(file);
});

app.get('/usuarios', (req, res) =>{
    res.setHeader('Content-type','text/json');
    const file = fs.readFileSync('./usuarios.json','UTF-8');
    res.send(file);
});

app.post('/buscador', (req, res) =>{
    let cadena = req.body.texto;
    const file = fs.readFileSync('./productos.json','UTF-8');
    try {
        const datos = JSON.parse(file);
        const regex = new RegExp(cadena, 'i');
        const objetosFiltrados = datos.filter(p => regex.test(p.nombre));
        fs.writeFile('./buscador.json', JSON.stringify(objetosFiltrados), 'utf8', err => {
            if (err) {
                console.log("Error al escribir el archivo:", err);
            }else{
                console.log("Archivo de resultados creado exitosamente");
                const buscador = fs.readFileSync('./buscador.json','UTF-8');
                res.send(buscador);
            }
        });
    } catch (err) {
        console.log("Error al analizar el archivo JSON:", err);
    }
});

app.get('/favoritos', (req, res) =>{
    const file = fs.readFileSync('./productos.json','UTF-8');
    try {
        const datos = JSON.parse(file);
        const objetosFiltrados = datos.filter(p => p.favorito == "true");
        fs.writeFile('./favoritos.json', JSON.stringify(objetosFiltrados), 'utf8', err => {
            if (err) {
                console.log("Error al escribir el archivo:", err);
            }else{
                console.log("Archivo de resultados creado exitosamente");
                const favoritos = fs.readFileSync('./favoritos.json','UTF-8');
                res.send(favoritos);
            }
        });
    } catch (err) {
        console.log("Error al analizar el archivo JSON:", err);
    }
});

app.get('/recomendados', (req, res) =>{
    const file = fs.readFileSync('./favoritos.json','UTF-8');
    try {
        const datos = JSON.parse(file);
        var numInformatica = 0;
        var numModa = 2;
        var numHogar = 0;
        var numLibros = 0;
        datos.filter(p => {
            if(p.categoria["nombre"] == "Moda") numModa++;
            else if(p.categoria["nombre"] == "Informatica") numInformatica++;
            else if(p.categoria["nombre"] == "HogarYCocina") numHogar++;
            else numLibros++;
        });

        var maxNum = Math.max(numInformatica, numLibros, numModa, numHogar);
        var maxCategoria;
        if(maxNum == numInformatica) maxCategoria = "Informatica";
        else if(maxNum == numLibros) maxCategoria = "Libros";
        else if(maxNum == numModa) maxCategoria = "Moda";
        else maxCategoria = "HogarYCocina";
        const fileProductos = fs.readFileSync('./productos.json','UTF-8');
        const datosProductos = JSON.parse(fileProductos);
        const objetosFiltrados = datosProductos.filter(p => p.categoria["nombre"] == maxCategoria);

        fs.writeFile('./recomendados.json', JSON.stringify(objetosFiltrados), 'utf8', err => {
            if (err) {
                console.log("Error al escribir el archivo:", err);
            }else{
                console.log("Archivo de resultados creado exitosamente");
                const recomendados = fs.readFileSync('./recomendados.json','UTF-8');
                res.send(recomendados);
            }
        });
    } catch (err) {
        console.log("Error al analizar el archivo JSON:", err);
    }
});

app.post('/filtrador', (req, res) =>{
    let cadena = req.body.filtro;
    const file = fs.readFileSync('./productos.json','UTF-8');
    try {
        const datos = JSON.parse(file);
        const objetosFiltrados = datos.filter(p => p.categoria["nombre"] == cadena);
        fs.writeFile('./filtro.json', JSON.stringify(objetosFiltrados), 'utf8', err => {
            if (err) {
                console.log("Error al escribir el archivo:", err);
            }else{
                console.log("Archivo de resultados creado exitosamente");
                const filtro = fs.readFileSync('./filtro.json','UTF-8');
                res.send(filtro);
            }
        });
    } catch (err) {
        console.log("Error al analizar el archivo JSON:", err);
    }
});

app.listen(3000, () =>{
    console.log('Servidor iniciado');
});