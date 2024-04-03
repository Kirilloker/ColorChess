const express = require("express");
const mysql = require("mysql2");
const morgan = require("morgan");
const cors = require("cors");
const appConfig = require("../Config/appConfig.json");

const app = express();
const port = appConfig.server.port;

const pool = mysql.createPool(appConfig.database).promise();

app.use(cors());
app.use(express.json());
app.use(morgan("dev"));

app.use((err, req, res, next) => {
  console.error(err.stack);
  res.status(500).send("Что-то пошло не так!");
});

app.get("/top", async (req, res) => {
  try {
    const [rows] = await pool.query(`
      SELECT u.Name, s.Win, s.Rate 
      FROM users u 
      JOIN userstatistics s ON u.Id = s.UserId 
      ORDER BY s.Rate DESC 
      LIMIT 5
    `);
    res.json(rows);
  } catch (err) {
    console.error(err);
    res.status(500).send("Ошибка сервера");
  }
});

app.get("/player/:nickname", async (req, res) => {
  try {
    const [rows] = await pool.query(
      `
      SELECT s.Win, s.Rate 
      FROM users u 
      JOIN userstatistics s ON u.Id = s.UserId 
      WHERE u.Name = ?
    `,
      [req.params.nickname]
    );

    if (rows.length > 0) {
      res.json(rows[0]);
    } else {
      res.status(404).send("Игрок не найден");
    }
  } catch (err) {
    console.error(err);
    res.status(500).send("Ошибка сервера");
  }
});

app.listen(port, appConfig.server.ip, () => {
  console.log(`Сервер запущен на http://${appConfig.server.ip}:${port}`);
});
