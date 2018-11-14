CREATE DATABASE `ced-orderdomain`;

CREATE TABLE `Order` (
  `Ref` CHAR(38) NOT NULL,
  `TotalValue` DECIMAL NOT NULL,
  `Created` DATETIME NOT NULL,
  `Updated` DATETIME NOT NULL,
  PRIMARY KEY (`ref`));


CREATE TABLE `OrderProduct` (
  `Ref` char(38) NOT NULL,
  `OrderRef` char(38) NOT NULL,
  `ProductName` varchar(100) NOT NULL,
  `Value` decimal(10,0) NOT NULL,
  PRIMARY KEY (`Ref`),
  CONSTRAINT `op_order` FOREIGN KEY (`OrderRef`) REFERENCES `Order` (`Ref`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

