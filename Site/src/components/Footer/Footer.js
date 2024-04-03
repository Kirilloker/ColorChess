// components/Footer/Footer.js
import React from 'react';
import './footer.css'; // Убедитесь, что у вас есть соответствующий CSS файл

function Footer() {
  return (
    <footer className="footer">
      <p>ColorChess ©2023</p>
      <a href="mailto:colorchess@yandex.ru">colorchess@yandex.ru</a>
      <a href="https://github.com/Kirilloker/ColorChess" target="_blank" rel="noopener noreferrer"> Source Code</a>
    </footer>
  );
}

export default Footer;
