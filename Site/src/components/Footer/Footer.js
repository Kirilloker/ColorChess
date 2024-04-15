import React from 'react';
import './footer.css';
import { useTranslation } from 'react-i18next';

function Footer() {
    const { t } = useTranslation();
    return (
    <footer className="footer">
      {/*<p>ColorChess ©2023</p>*/}
      <a href="mailto:colorchess@yandex.ru">colorchess@yandex.ru</a>
          <a href="https://github.com/Kirilloker/ColorChess" target="_blank" rel="noopener noreferrer"> {t('SourceCode')} </a>
    </footer>
  );
}

export default Footer;
