import React, { useState, useEffect } from "react";
import { useTranslation } from 'react-i18next';
import "./top.css";

function Top() {
    const { t } = useTranslation();

    const [players, setPlayers] = useState([]);
    const [nickname, setNickname] = useState("");
    const [playerInfo, setPlayerInfo] = useState(null);
    const [errorMessage, setErrorMessage] = useState("");

    const serverIP = process.env.REACT_APP_SERVER_IP;
    const serverPort = process.env.REACT_APP_SERVER_PORT;

    useEffect(() => {
        fetch(`http://${serverIP}:${serverPort}/list_top`)
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
                    <h1>{t('TopPlayers')}</h1>
                    <div className="players-list">
                        <table>
                            <thead>
                                <tr>
                                    <th>{t('Nickname')}</th>
                                    <th>{t('Rating')}</th>
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
                            placeholder={t('EnterNick')}
                            value={nickname}
                            onChange={(e) => setNickname(e.target.value)}
                        />
                        <button onClick={handleSearch}>{t('PlayerStats')}</button>
                    </div>
                    {errorMessage && <div className="error-message">{errorMessage}</div>}
                    {playerInfo && (
                        <div className="player-stats-result">
                            {t('NumVictories')} {playerInfo.wins} {t('VictoryDetails1')} {playerInfo.rate} {t('VictoryDetails2')} {playerInfo.numberPlace}
                        </div>
                    )}
                </section>
            </div>
        </main>
    );
}

export default Top;
