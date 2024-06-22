const express = require('express');
const path = require('path');

const app = express();
const port = 8080;

// Разрешить доступ к статическим файлам вашего Svelte-приложения
app.use(express.static(path.join(__dirname, 'public')));

// Все запросы будут возвращать главный HTML-файл
app.get('*', (req, res) => {
  res.sendFile(path.join(__dirname, 'public', 'index.html'));
});

// Слушаем указанный порт
app.listen(port, () => {
  console.log(`Сервер запущен на порту ${port}`);
});