const express = require('express');
const mysql = require('mysql2');

const app = express();
const port = 3000;

// Подключение к MySQL
const pool = mysql.createPool({
  host: 'localhost',
  user: 'kirillok',
  database: 'colorchessdb',
  password: 'loki5566'
}).promise();
const cors = require('cors');
app.use(cors());

app.use(express.json());

// Получение топ-5 игроков
app.get('/top', async (req, res) => {
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
    res.status(500).send('Ошибка сервера');
  }
});

// Поиск игрока по никнейму
app.get('/player/:nickname', async (req, res) => {
  try {
    const [rows] = await pool.query(`
      SELECT s.Win, s.Rate 
      FROM users u 
      JOIN userstatistics s ON u.Id = s.UserId 
      WHERE u.Name = ?
    `, [req.params.nickname]);

    if (rows.length > 0) {
      res.json(rows[0]);
    } else {
      res.status(404).send('Игрок не найден');
    }
  } catch (err) {
    console.error(err);
    res.status(500).send('Ошибка сервера');
  }
});

app.listen(port, () => {
  console.log(`Сервер запущен на http://localhost:${port}`);
});
