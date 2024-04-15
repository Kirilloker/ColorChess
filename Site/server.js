const express = require("express");
const morgan = require("morgan");
const cors = require("cors");
const axios = require("axios");
const path = require("path");

require("dotenv").config();

const app = express();
const portServer = process.env.SERVER_PORT;
const ipServer = process.env.SERVER_IP;
const urlAPI = process.env.URL_API;

app.use(cors());
app.use(express.json());
app.use(morgan("dev"));
app.use(express.static(path.resolve(__dirname, "build")));  


app.get("/list_top", async (req, res) => {
  try {
    const response = await axios.get(`http://${urlAPI}/api/Info/get_top`, {
      params: { nameUser: "." },
    });
    res.json(response.data);
  } catch (err) {
    console.error(err);
    res.status(err.response ? err.response.status : 500).send("Ошибка сервера");
  }
});

app.get("/player/:nickname", async (req, res) => {
  try {
    const response = await axios.get(
      `http://${urlAPI}/api/Info/get_info_users`,
      {
        params: { nameUser: req.params.nickname },
      }
    );

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

// Handler for all other requests, should return index.html
app.get("*", (req, res) => {
  res.sendFile(path.resolve(__dirname, "build", "index.html"));
});

// Global error handler
app.use((err, req, res, next) => {
  console.error(err.stack);
  res.status(500).send("Что-то пошло не так!");
});

app.listen(portServer, ipServer, () => {
  console.log(`Сервер запущен на http://${ipServer}:${portServer}`);
});
