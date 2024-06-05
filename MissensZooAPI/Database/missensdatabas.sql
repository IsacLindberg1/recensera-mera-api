-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jan 26, 2024 at 09:10 AM
-- Server version: 10.4.28-MariaDB
-- PHP Version: 8.2.4

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `missensdatabas`
--

-- --------------------------------------------------------

--
-- Table structure for table `blogpost`
--

CREATE TABLE `blogpost` (
  `blogId` int(32) NOT NULL,
  `title` varchar(100) NOT NULL,
  `userId` int(32) NOT NULL,
  `content` varchar(3000) NOT NULL,
  `image` varchar(220) NOT NULL,
  `timestamp` varchar(32) NOT NULL DEFAULT current_timestamp(),
  `category` varchar(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `blogpost`
--

INSERT INTO `blogpost` (`blogId`, `title`, `userId`, `content`, `image`, `timestamp`, `category`) VALUES
(6, '1234', 7, '1234', '1234', '2024-01-12 10:06:30', '1234'),
(7, 'hej', 7, '123', '123', '2024-01-12 10:20:53', '123'),
(8, 'string', 7, 'string', 'string', '2024-01-12 10:22:25', 'string'),
(9, 'How to escape Granny\'s House', 7, 'Grannys house, i dont wannna stay, i just wanna go home, GRANNYS HOUSE!!!! ', 'granny.png', '2024-01-12 14:42:51', 'Survival Skillz'),
(13, 'string', 7, 'string', 'string', '2024-01-15 15:17:49', 'string');

-- --------------------------------------------------------

--
-- Table structure for table `cart`
--

CREATE TABLE `cart` (
  `id` int(128) NOT NULL,
  `userId` int(32) NOT NULL,
  `productId` int(16) NOT NULL,
  `quantity` int(8) NOT NULL,
  `price` int(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `cart`
--

INSERT INTO `cart` (`id`, `userId`, `productId`, `quantity`, `price`) VALUES
(1, 7, 1, 1, 0),
(3, 7, 2, 3, 0),
(4, 8, 2, 7, 50),
(5, 8, 2, 20, 20),
(6, 0, 0, 0, 0);

-- --------------------------------------------------------

--
-- Table structure for table `comments`
--

CREATE TABLE `comments` (
  `id` int(32) NOT NULL,
  `blogId` int(32) NOT NULL,
  `userId` int(32) NOT NULL,
  `content` varchar(500) NOT NULL,
  `Timestamp` timestamp(6) NOT NULL DEFAULT current_timestamp(6) ON UPDATE current_timestamp(6)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `orderproducts`
--

CREATE TABLE `orderproducts` (
  `id` int(32) NOT NULL,
  `userId` int(32) NOT NULL,
  `productId` int(16) NOT NULL,
  `quantity` int(32) NOT NULL,
  `price` int(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `orderproducts`
--

INSERT INTO `orderproducts` (`id`, `userId`, `productId`, `quantity`, `price`) VALUES
(23, 8, 2, 7, 50),
(24, 8, 2, 20, 20);

-- --------------------------------------------------------

--
-- Table structure for table `orders`
--

CREATE TABLE `orders` (
  `orderId` int(32) NOT NULL,
  `userId` int(32) NOT NULL,
  `price` int(32) NOT NULL,
  `phonenumber` varchar(20) NOT NULL,
  `zipcode` varchar(15) NOT NULL,
  `country` varchar(56) NOT NULL,
  `city` varchar(100) NOT NULL,
  `adress` varchar(80) NOT NULL,
  `recipientName` varchar(80) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `orders`
--

INSERT INTO `orders` (`orderId`, `userId`, `price`, `phonenumber`, `zipcode`, `country`, `city`, `adress`, `recipientName`) VALUES
(1, 8, 700, '070000000', '123123', 'SVERIGE', 'LJUNGBY', 'Harabergsgatan 4', 'ISAC LINDBERG'),
(2, 8, 650, '1111111', '1111', 'SVERIGE', 'VAXJO', '123 TEKNIKUM', 'DAVID'),
(3, 8, 100, '123123', '123123', 'SWEDEN', 'DANMARK', '123132', 'TIM TUVESTAM');

-- --------------------------------------------------------

--
-- Table structure for table `product`
--

CREATE TABLE `product` (
  `id` int(16) NOT NULL,
  `description` varchar(220) NOT NULL,
  `name` varchar(40) NOT NULL,
  `category` varchar(20) NOT NULL,
  `price` int(16) NOT NULL,
  `stock` int(16) NOT NULL,
  `image` varchar(220) NOT NULL,
  `rating` int(8) NOT NULL,
  `ratingCount` int(32) NOT NULL,
  `portionPerDay` int(8) NOT NULL,
  `foodInfo` varchar(128) NOT NULL,
  `onSale` tinyint(1) NOT NULL,
  `salePercentage` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `product`
--

INSERT INTO `product` (`id`, `description`, `name`, `category`, `price`, `stock`, `image`, `rating`, `ratingCount`, `portionPerDay`, `foodInfo`, `onSale`, `salePercentage`) VALUES
(3, 'Perfekt för torr häst-hud', 'Hästolja', 'skönhetsprodukter', 200, 412, 'image6.png', 5, 0, 0, '0', 1, 10),
(7, 'Värmer din katts små tassar', 'Kattstrumpor', 'annat', 249, 361, 'image1.png', 3, 125, 0, '', 0, 0),
(8, 'Små boxningshandskar för din håriga lilla vän', 'Boxningshandskar för hamster', 'annat', 126, 42, 'image2.png', 4, 123, 0, '', 0, 0),
(9, 'En eka i furu gjord för att transportera rådjur', 'Eka för rådjur', 'annat', 980, 12, 'image3.png', 4, 42, 0, '', 0, 0),
(10, 'En krigstränad hund redo att beskydda dig', 'Soldat-hund', 'djur', 7900, 4, 'image5.png', 5, 12, 0, '', 0, 0),
(11, 'Delikat bakelse som lever!', 'Hamster muffin', 'djur', 340, 50, 'image7.png', 4, 42, 0, '', 1, 20),
(12, 'Livsfarligt odjur som föds upp i Morias grottor', 'Demoniskt odjur', 'djur', 790, 4891, 'image8.png', 1, 7, 0, '', 1, 99),
(13, 'En livs levande Vätte som gillar att busa', 'Vätte', 'djur', 250, 57, 'image9.png', 3, 54, 0, '', 0, 0);

-- --------------------------------------------------------

--
-- Table structure for table `user`
--

CREATE TABLE `user` (
  `id` int(32) NOT NULL,
  `role` varchar(8) NOT NULL,
  `userName` varchar(20) NOT NULL,
  `password` varchar(64) NOT NULL,
  `email` varchar(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `user`
--

INSERT INTO `user` (`id`, `role`, `userName`, `password`, `email`) VALUES
(7, '2', 'David wernow', '123', '123'),
(8, '3', 'Admin', '$2a$11$r7Gd9S6bY08hLTGf805meOGJ/UcYFHVxeOuT9j0xrN.coFX/b3Dwe', 'Admin');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `blogpost`
--
ALTER TABLE `blogpost`
  ADD PRIMARY KEY (`blogId`);

--
-- Indexes for table `cart`
--
ALTER TABLE `cart`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `comments`
--
ALTER TABLE `comments`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `orderproducts`
--
ALTER TABLE `orderproducts`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `orders`
--
ALTER TABLE `orders`
  ADD PRIMARY KEY (`orderId`);

--
-- Indexes for table `product`
--
ALTER TABLE `product`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `blogpost`
--
ALTER TABLE `blogpost`
  MODIFY `blogId` int(32) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT for table `cart`
--
ALTER TABLE `cart`
  MODIFY `id` int(128) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT for table `comments`
--
ALTER TABLE `comments`
  MODIFY `id` int(32) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `orderproducts`
--
ALTER TABLE `orderproducts`
  MODIFY `id` int(32) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=25;

--
-- AUTO_INCREMENT for table `orders`
--
ALTER TABLE `orders`
  MODIFY `orderId` int(32) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `product`
--
ALTER TABLE `product`
  MODIFY `id` int(16) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT for table `user`
--
ALTER TABLE `user`
  MODIFY `id` int(32) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
