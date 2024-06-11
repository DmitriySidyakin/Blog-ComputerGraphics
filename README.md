# Компьютерная графика

Приложения на C# для объяснения компьютерной графики

## Решение на C# для Кривых Безье

Приложение генерирует кадры для анимации в текущем каталоге исполняемого файла. Приложение написано на C# WPF.
Размер кадра: 400 на 200 точек. Не учитывается уровень дискретизации t, далее в статье написано как рассчитать шаг для Кривых Безье старше 1-го порядка на примере кривых 1-го порядка. 

Подкаталог: https://github.com/DmitriySidyakin/Blog-ComputerGraphics/tree/master/Bezier/BezierSolution

Русскоязычное Описание алгоритма: [Bezier-curves](https://dmitriysidyakin.github.io/School-IT/csharp-articles/ru-ru/algorithms-on-csharp/articles/0001-Bezier-curves/)

### Проект рендерит следующие картинки:

![4 points Bezier Curve](https://github.com/DmitriySidyakin/Blog-ComputerGraphics/blob/master/Documentations/img/b3a.gif)

![4 points De Casteljau's Bezier Curve](https://github.com/DmitriySidyakin/Blog-ComputerGraphics/blob/master/Documentations/img/b3adc.gif)

![5 points Bezier Curve](https://github.com/DmitriySidyakin/Blog-ComputerGraphics/blob/master/Documentations/img/b4a.gif)

![5 points De Casteljau's Bezier Curve](https://github.com/DmitriySidyakin/Blog-ComputerGraphics/blob/master/Documentations/img/b4adc.gif)

Изображения генерируются покадрово, кадры находятся в корне приложения.

## Решение на C# для начала Движка для трёхмерной визуализации функциональных повержностей

Приложение представлет собой Движок для рендеринга функциональных объектов на процессоре, без испльзования Видеокарты. Всё построение основано на хранении цветов в 3-х мерном массиве, который представляет собой мир. Главное правильно направить камеру.

Подкаталог: https://github.com/DmitriySidyakin/Blog-ComputerGraphics/tree/master/3DSamles/

Русскоязычное Описание алгоритма: [3D-on-CPU](https://dmitriysidyakin.github.io/School-IT/csharp-articles/ru-ru/algorithms-on-csharp/articles/0004-3D-on-CPU/)

### Проект рендерит следующие картинки:

![Cube](https://github.com/DmitriySidyakin/Blog-ComputerGraphics/blob/master/Documentations/img/cube.png)

![Bezier Surface](https://github.com/DmitriySidyakin/Blog-ComputerGraphics/blob/master/Documentations/img/BezierSurface.png)

Изображения генерируются в корне приложения.

# Примечание
Приложение позволяет отобразить любые объекты без прозрачности. Они не отделены от примеров.
Рендеринг одного среднего кадра длится порядка месяца для 3D-графики. Для Кривых Безье, рендеринг порядка часа.

Маловероятно, что приложения будут развиваться, но Вы можете их оптимизировать, ускорить, и отталкиваться от них.

**Рендеринг не возможен на процессоре по пикселям - это долго, только если Вы не создаёте для себя математические изображения фотофильтры, которые работают допустим в консольном приложении для создания уникальной картинки для себя/распространения.**
