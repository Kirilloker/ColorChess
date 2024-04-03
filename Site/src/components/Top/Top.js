import React, { useState, useEffect } from "react";
import "./top.css";

function Top() {
    const [players, setPlayers] = useState([]);
    const [nickname, setNickname] = useState("");
    const [playerInfo, setPlayerInfo] = useState(null);
    const [errorMessage, setErrorMessage] = useState("");

    const serverIP = process.env.REACT_APP_SERVER_IP;
    const serverPort = process.env.REACT_APP_SERVER_PORT;

    useEffect(() => {
        fetch(`http://${serverIP}:${serverPort}/top`)
            .then((response) => response.json())
            .then((data) => setPlayers(data))
            .catch((error) => console.error("Ошибка загрузки топ игроков:", error));
    }, [serverIP, serverPort]);

    const handleSearch = () => {
        fetch(`http://${serverIP}:${serverPort}/player/${nickname}`)
            .then((response) => {
                if (!response.ok) {
                    throw new Error("Игрок не найден");
                }
                return response.json();
            })
            .then((data) => {
                setPlayerInfo(data);
                setErrorMessage("");
            })
            .catch((error) => {
                console.error("Ошибка поиска игрока:", error);
                setPlayerInfo(null);
                setErrorMessage("Игрок не найден");

                setTimeout(() => {
                    setErrorMessage("");
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
                                    <th>Rating</th>
                                </tr>
                            </thead>
                            <tbody>
                                {players.map((player, index) => (
                                    <tr key={index}>
                                        <td>{player.first}</td>
                                        <td>{player.second}</td>
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
                            onChange={(e) => setNickname(e.target.value)}
                        />
                        <button onClick={handleSearch}>Find out a player's stats</button>
                    </div>
                    {errorMessage && <div className="error-message">{errorMessage}</div>}
                    {playerInfo && (
                        <div className="player-stats-result">
                            Number of victories: {playerInfo.wins} - Rating: {playerInfo.rate} - Place in Top: {playerInfo.numberPlace}
                        </div>
                    )}
                </section>
            </div>
        </main>
    );
}

export default Top;
