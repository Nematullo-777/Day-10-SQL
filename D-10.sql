CREATE TABLE movies (
    id SERIAL PRIMARY KEY,
    title VARCHAR(150) NOT NULL,
    director VARCHAR(100),
    year INTEGER,
    duration INTEGER,
    genre VARCHAR(50),
    description TEXT
);

CREATE TABLE theaters (
    id SERIAL PRIMARY KEY,
    name VARCHAR(150) NOT NULL,
    location VARCHAR(150),
    manager VARCHAR(150),
    phone VARCHAR(30),
    capacity INTEGER
);

CREATE TABLE screenings (
    id SERIAL PRIMARY KEY,
    movie_id INTEGER REFERENCES movies(id) ON DELETE CASCADE,
    theater_id INTEGER REFERENCES theaters(id) ON DELETE CASCADE,
    screening_time TIMESTAMP NOT NULL,
    ticket_price DECIMAL(6,2) NOT NULL,
    available_seats INTEGER NOT NULL
);

CREATE TABLE tickets (
    id SERIAL PRIMARY KEY,
    screening_id INTEGER REFERENCES screenings(id) ON DELETE CASCADE,
    customer_name VARCHAR(150) NOT NULL,
    seat_number VARCHAR(10),
    price DECIMAL(6,2) NOT NULL
);
INSERT INTO movies (title, director, year, duration, genre, description) VALUES
('The Shawshank Redemption', 'Frank Darabont', 1994, 142, 'Drama', 'История о надежде и дружбе в тюрьме.'),
('The Godfather', 'Francis Ford Coppola', 1972, 175, 'Crime', 'Эпическая сага о мафиозном клане.'),
('The Dark Knight', 'Christopher Nolan', 2008, 152, 'Action', 'Бэтмен против безумного Джокера.'),
('Pulp Fiction', 'Quentin Tarantino', 1994, 154, 'Crime', 'Переплетение историй о жизни преступников.'),
('Forrest Gump', 'Robert Zemeckis', 1994, 142, 'Drama', 'Невероятное путешествие простодушного мужчины.'),
('Inception', 'Christopher Nolan', 2010, 148, 'Sci-Fi', 'Крадущийся в снах преступник с уникальной миссией.'),
('Fight Club', 'David Fincher', 1999, 139, 'Drama', 'Подпольный бойцовский клуб как способ освобождения.'),
('The Matrix', 'Lana Wachowski, Lilly Wachowski', 1999, 136, 'Sci-Fi', 'Хакер открывает для себя реальность, отличную от привычной.'),
('Goodfellas', 'Martin Scorsese', 1990, 146, 'Crime', 'История взлёта и падения мафиозного соучастника.'),
('Interstellar', 'Christopher Nolan', 2014, 169, 'Sci-Fi', 'Группа исследователей отправляется в космическое путешествие.');
INSERT INTO theaters (name, location, manager, phone, capacity) VALUES
('Cineplex 1', 'Душанбе', 'Ali Rahimov', '+9921234567', 150),
('Cinema Star', 'Худжанд', 'Leyla Karimova', '+9922345678', 200),
('Movie Palace', 'Бухара', 'Rashid Mirza', '+9923456789', 250),
('Golden Screen', 'Самарканд', 'Olga Ivanova', '+9924567890', 180),
('Film House', 'Ташкент', 'Akmal Saidov', '+998901234567', 220),
('Silver Cinema', 'Бишкек', 'Aida Bekova', '+996555123456', 160),
('Premiere Theater', 'Алма-Ата', 'Nursultan Nazarbayev', '+77271234567', 300),
('Star Movies', 'Душанбе', 'Farhod Saidov', '+9925678901', 190),
('Cinema World', 'Худжанд', 'Marina Petrova', '+9926789012', 210),
('Epic Screen', 'Душанбе', 'Rustam Aliyev', '+9927890123', 170);
INSERT INTO screenings (movie_id, theater_id, screening_time, ticket_price, available_seats) 
VALUES(1, 1, '2025-03-15 18:00:00', 10.00, 150),
(2, 2, '2025-03-15 20:00:00', 12.50, 200),
(3, 3, '2025-03-16 19:30:00', 11.00, 250),
(4, 4, '2025-03-16 21:00:00', 9.50, 180),
(5, 5, '2025-03-17 17:00:00', 10.50, 220),
(6, 6, '2025-03-17 20:30:00', 13.00, 160),
(7, 7, '2025-03-18 18:45:00', 12.00, 300),
(8, 8, '2025-03-18 19:00:00', 11.50, 190),
(9, 9, '2025-03-19 20:15:00', 10.75, 210),
(10, 10, '2025-03-19 21:30:00', 14.00, 170);
INSERT INTO tickets (screening_id, customer_name, seat_number, price) 
VALUES(1, 'Ivan Petrov', 'A1', 10.00),
(2, 'Maria Ivanova', 'B2', 12.50),
(3, 'Alexey Sidorov', 'C3', 11.00),
(4, 'Elena Smirnova', 'D4', 9.50),
(5, 'Sergey Kuznetsov', 'E5', 10.50),
(6, 'Natalia Volkova', 'F6', 13.00),
(7, 'Dmitry Orlov', 'G7', 12.00),
(8, 'Olga Morozova', 'H8', 11.50),
(9, 'Pavel Fedorov', 'I9', 10.75),
(10, 'Svetlana Nikitina', 'J10', 14.00);
SELECT m.title, COUNT(s.id) AS total_screenings
FROM movies m
LEFT JOIN screenings s ON m.id = s.movie_id
GROUP BY m.title;