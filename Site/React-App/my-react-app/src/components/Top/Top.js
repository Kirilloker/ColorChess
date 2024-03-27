import React, { useState, useEffect } from 'react';
import './top.css'; // Убедитесь, что путь к CSS файлу корректный

function Top() {
  const [players, setPlayers] = useState([]);
  const [nickname, setNickname] = useState('');
  const [playerInfo, setPlayerInfo] = useState(null);
  const [errorMessage, setErrorMessage] = useState(''); // Добавляем состояние для сообщения об ошибке

  useEffect(() => {
    fetch('http://localhost:3000/top')
      .then(response => response.json())
      .then(data => setPlayers(data))
      .catch(error => console.error('Ошибка загрузки топ игроков:', error));
  }, []);

  const handleSearch = () => {
    fetch(`http://localhost:3000/player/${nickname}`)
      .then(response => {
        if (!response.ok) {
          throw new Error('Игрок не найден'); // Если ответ сервера не ок, выбрасываем ошибку
        }
        return response.json();
      })
      .then(data => {
        setPlayerInfo(data);
        setErrorMessage(''); // Очищаем сообщение об ошибке, если запрос успешен
      })
      .catch(error => {
        console.error('Ошибка поиска игрока:', error);
        setPlayerInfo(null); // Очищаем информацию о игроке
        setErrorMessage('Игрок не найден'); // Устанавливаем сообщение об ошибке

        // Задержка в 3 секунды перед удалением сообщения об ошибке
        setTimeout(() => {
          setErrorMessage('');
        }, 2000);
      });
  };

  return (
    <main>
      <div className="container">
        <section className="top-players">
          <h1>Table of the Best Players</h1>
          <div className="players-list">
            <table>
              <thead>
                <tr>
                  <th>Nickname</th>
                  <th>Number of Victories</th>
                  <th>Rating</th>
                </tr>
              </thead>
              <tbody>
                {players.map((player, index) => (
                  <tr key={index}>
                    <td>{player.Name}</td>
                    <td>{player.Win}</td>
                    <td>{player.Rate}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
		  <br />
		  <br />
		  <br />
          <div className="player-stats-input">
            <input
              type="text"
              placeholder="Enter a nickname"
              value={nickname}
              onChange={e => setNickname(e.target.value)}
            />
            <button onClick={handleSearch}>Find out a player's stats</button>
          </div>
          {errorMessage && <div className="error-message">{errorMessage}</div>}
          {playerInfo && (
            <div className="player-stats-result">
              Number of victories: {playerInfo.Win} - Rating: {playerInfo.Rate}
            </div>
          )}
        </section>
      </div>
    </main>
  );
}

export default Top;
