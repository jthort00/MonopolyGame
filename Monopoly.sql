DROP DATABASE IF EXISTS Monopoly;
CREATE DATABASE Monopoly;
USE Monopoly;

CREATE TABLE Games (
 gameID INT,
 status VARCHAR(50),
 player1 VARCHAR(50),
 player2 VARCHAR(50),
 player3 VARCHAR(50),
 player4 VARCHAR(50),
 player5 VARCHAR(50),
 Player6 VARCHAR(50),
 PRIMARY KEY (gameID)
)ENGINE=InnoDB;

CREATE TABLE Credentials (
 playerID VARCHAR(50),
 pass VARCHAR(50),
 age INT,
 PRIMARY KEY (playerID)
)ENGINE=InnoDB;

CREATE TABLE Players (
 gameID INT NOT NULL,
 playerID VARCHAR(50),
 PRIMARY KEY (gameID,playerID),
 FOREIGN KEY (gameID) REFERENCES Games(gameID) ON DELETE CASCADE ON UPDATE CASCADE,
 FOREIGN KEY (playerID) REFERENCES Credentials(playerID)
)ENGINE=InnoDB;

CREATE TABLE PlayerStatus (
 gameID INT NOT NULL,
 playerID VARCHAR(50),
 money INT,
 properties INT,
 specialcard INT,
 jail INT,
 position INT,
 PRIMARY KEY (gameID,playerID),
 FOREIGN KEY (gameID) REFERENCES Games(gameID) ON DELETE CASCADE ON UPDATE CASCADE,
 FOREIGN KEY (playerID) REFERENCES Credentials(playerID)
)ENGINE=InnoDB;


CREATE TABLE Properties (
 propertyID INT NOT NULL,
 location INT NOT NULL,
 name VARCHAR(50),
 price INT, 
 fee INT,
 PRIMARY KEY (propertyID)
)ENGINE=InnoDB;

CREATE TABLE PropertyAssign (
 gameID INT,
 playerID VARCHAR(50),
 propertyID INT,
 constructions INT,
 mortgaged INT,
 FOREIGN KEY (gameID) REFERENCES Games(gameID),
 FOREIGN KEY (playerID) REFERENCES Credentials(playerID),
 FOREIGN KEY (propertyID) REFERENCES Properties(propertyID)
)ENGINE=InnoDB;





 


/*Games
INSERT INTO Games VALUES (1,'Playing');
INSERT INTO Games VALUES (2,'Playing');
INSERT INTO Games VALUES (3,'Playing');

/*Players and games that are playing
INSERT INTO Players VALUES (1,'player1');
INSERT INTO Players VALUES (2,'player1');
INSERT INTO Players VALUES (1,'player2');
INSERT INTO Players VALUES (1,'player3');
INSERT INTO Players VALUES (1,'player4');
INSERT INTO Players VALUES (2,'player4');
INSERT INTO Players VALUES (2,'player5');

/*Status of each player in each game
INSERT INTO PlayerStatus VALUES (1,'player1',500,'temporary',0,0,20);
INSERT INTO PlayerStatus VALUES (2,'player1',1200, 'temporary',1,1,2);
INSERT INTO PlayerStatus VALUES (1,'player2',100, 'temporary', 0,0,24);
INSERT INTO PlayerStatus VALUES (1,'player3',2000, 'temporary', 0, 1,4);
INSERT INTO PlayerStatus VALUES (1,'player4',4000, 'temporary', 1, 0,12);
INSERT INTO PlayerStatus VALUES (2,'player4',3250, 'temporary', 0, 0,18);
INSERT INTO PlayerStatus VALUES (2,'player5',35, 'temporary', 0, 1,15);




/* Dame los ID de los jugadores y de la partida donde estan con más de 100 euros que estan en prisión */

/*SELECT Games.gameID, PlayerStatus.playerID FROM Games, PlayerStatus WHERE Games.gameID = PlayerStatus.gameID AND PlayerStatus.jail = 1 AND PlayerStatus.money > 100;*\


