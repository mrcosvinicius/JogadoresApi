-- Seed de dados: 2 ligas, 3 times cada, 5 jogadores por time.
-- Respeita as validacoes do Model (apenas letras e espacos em nomes/posicoes).
-- Execute: mysql -u<user> -p<senha> -D Jogadores < seed.sql

-- ===================== LIGAS =====================
INSERT INTO Ligas (Nome) VALUES
  ('La Liga'),
  ('Premier League');

SET @laLiga  = (SELECT Id FROM Ligas WHERE Nome = 'La Liga');
SET @premier = (SELECT Id FROM Ligas WHERE Nome = 'Premier League');

-- ===================== TIMES =====================
INSERT INTO Times (Nome, Estadio) VALUES
  ('Real Madrid',       'Santiago Bernabeu'),
  ('Barcelona',         'Camp Nou'),
  ('Atletico Madrid',   'Metropolitano'),
  ('Manchester United', 'Old Trafford'),
  ('Liverpool',         'Anfield'),
  ('Chelsea',           'Stamford Bridge');

-- Associa cada time a sua liga (tabela de juncao N:N)
INSERT INTO LigaTime (LigasId, TimesId)
  SELECT @laLiga, Id FROM Times WHERE Nome IN ('Real Madrid', 'Barcelona', 'Atletico Madrid');

INSERT INTO LigaTime (LigasId, TimesId)
  SELECT @premier, Id FROM Times WHERE Nome IN ('Manchester United', 'Liverpool', 'Chelsea');

-- ===================== JOGADORES =====================
-- Real Madrid
INSERT INTO Jogadores (Nome, Posicao, Gols, TimeId)
  SELECT t.Nome, t.Posicao, t.Gols, (SELECT Id FROM Times WHERE Nome = 'Real Madrid') FROM (
    SELECT 'Vinicius Junior' AS Nome, 'Atacante'   AS Posicao, 80 AS Gols UNION
    SELECT 'Jude Bellingham',         'Meio Campo',           40        UNION
    SELECT 'Rodrygo',                 'Atacante',             30        UNION
    SELECT 'Luka Modric',             'Meio Campo',           20        UNION
    SELECT 'Thibaut Courtois',        'Goleiro',              0
  ) t;

-- Barcelona
INSERT INTO Jogadores (Nome, Posicao, Gols, TimeId)
  SELECT t.Nome, t.Posicao, t.Gols, (SELECT Id FROM Times WHERE Nome = 'Barcelona') FROM (
    SELECT 'Robert Lewandowski' AS Nome, 'Atacante' AS Posicao, 90 AS Gols UNION
    SELECT 'Pedri',                       'Meio Campo',          15        UNION
    SELECT 'Gavi',                        'Meio Campo',          10        UNION
    SELECT 'Raphinha',                    'Atacante',            35        UNION
    SELECT 'Marc Andre ter Stegen',       'Goleiro',             0
  ) t;

-- Atletico Madrid
INSERT INTO Jogadores (Nome, Posicao, Gols, TimeId)
  SELECT t.Nome, t.Posicao, t.Gols, (SELECT Id FROM Times WHERE Nome = 'Atletico Madrid') FROM (
    SELECT 'Antoine Griezmann' AS Nome, 'Atacante' AS Posicao, 70 AS Gols UNION
    SELECT 'Marcos Llorente',           'Meio Campo',          20        UNION
    SELECT 'Koke',                      'Meio Campo',          15        UNION
    SELECT 'Alvaro Morata',             'Atacante',            45        UNION
    SELECT 'Jan Oblak',                 'Goleiro',             0
  ) t;

-- Manchester United
INSERT INTO Jogadores (Nome, Posicao, Gols, TimeId)
  SELECT t.Nome, t.Posicao, t.Gols, (SELECT Id FROM Times WHERE Nome = 'Manchester United') FROM (
    SELECT 'Bruno Fernandes' AS Nome, 'Meio Campo' AS Posicao, 50 AS Gols UNION
    SELECT 'Marcus Rashford',          'Atacante',             60        UNION
    SELECT 'Rasmus Hojlund',           'Atacante',             25        UNION
    SELECT 'Casemiro',                 'Meio Campo',           15        UNION
    SELECT 'Andre Onana',              'Goleiro',              0
  ) t;

-- Liverpool
INSERT INTO Jogadores (Nome, Posicao, Gols, TimeId)
  SELECT t.Nome, t.Posicao, t.Gols, (SELECT Id FROM Times WHERE Nome = 'Liverpool') FROM (
    SELECT 'Mohamed Salah' AS Nome, 'Atacante' AS Posicao, 85 AS Gols UNION
    SELECT 'Darwin Nunez',            'Atacante',            40        UNION
    SELECT 'Alexis Mac Allister',     'Meio Campo',          20        UNION
    SELECT 'Virgil van Dijk',         'Zagueiro',            5         UNION
    SELECT 'Alisson',                 'Goleiro',             0
  ) t;

-- Chelsea
INSERT INTO Jogadores (Nome, Posicao, Gols, TimeId)
  SELECT t.Nome, t.Posicao, t.Gols, (SELECT Id FROM Times WHERE Nome = 'Chelsea') FROM (
    SELECT 'Cole Palmer' AS Nome, 'Meio Campo' AS Posicao, 45 AS Gols UNION
    SELECT 'Nicolas Jackson',        'Atacante',             30        UNION
    SELECT 'Enzo Fernandez',         'Meio Campo',           15        UNION
    SELECT 'Reece James',            'Lateral',              5         UNION
    SELECT 'Robert Sanchez',         'Goleiro',              0
  ) t;
