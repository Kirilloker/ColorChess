const express = require("express");
const morgan = require("morgan");
const cors = require("cors");
const axios = require("axios"); 

require('dotenv').config();

const app = express();
const portServer = process.env.SERVER_PORT;
const ipServer = process.env.SERVER_IP;
const urlAPI = process.env.URL_API;

app.use(cors());
app.use(express.json());
app.use(morgan("dev"));

app.use((err, req, res, next) => {
  console.error(err.stack);
  res.status(500).send("Что-то пошло не так!");
});

app.get("/top", async (req, res) => {
  try {  
    const response = await axios.get(`http://${urlAPI}/api/Info/get_top`, {
      params: { nameUser: "." } 
    });
    res.json(response.data);
  } catch (err) {
    console.error(err);
    res.status(err.response ? err.response.status : 500).send("Ошибка сервера");
  }
});

app.get("/player/:nickname", async (req, res) => {
  try {
    const response = await axios.get(`http://${urlAPI}/api/Info/get_info_users`, {
      params: { nameUser: req.params.nickname }
    });

    if (response.data) {
      res.json(response.data);
    } else {
      res.status(404).send("Игрок не найден");
    }
  } catch (err) {
    console.error(err);
    res.status(err.response ? err.response.status : 500).send("Ошибка сервера");
  }
});

app.use(express.static('build/folder'));

app.get('*', (req, res) => {
    res.sendFile('build/folder/index.html');
});

app.listen(portServer, ipServer, () => {
    console.log(`Сервер запущен на http://${ipServer}:${portServer}`);
});

