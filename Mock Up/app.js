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

app.listen(3000, () =>{
    console.log('Servidor iniciado');
});