// index.js
import React from 'react';
import ReactDOM from 'react-dom';
import './index.css'; // Глобальные стили, возможно, тут будет подключение common.css
import App from './App';
import reportWebVitals from './reportWebVitals';

ReactDOM.render(
  <React.StrictMode>
    <App />
  </React.StrictMode>,
  document.getElementById('root')
);

// Если вы хотите начать измерять производительность в вашем приложении, передайте функцию
// для логирования результатов (например: reportWebVitals(console.log))
// или отправьте в аналитику. Узнайте больше: https://bit.ly/CRA-vitals
reportWebVitals();
