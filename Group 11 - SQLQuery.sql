USE freshsourcedb;
GO

-------------------------------------------------------------------------------
-- 1. SEED SUPPLIERS (10 rows)
-------------------------------------------------------------------------------

INSERT INTO Suppliers (Name, ContactEmail, Phone)
VALUES  
('FreshSource Toronto', 'toronto@freshsource.com', '416-111-1111'),
('FreshSource Mississauga', 'miss@freshsource.com', '905-222-2222'),
('FreshSource Markham', 'markham@freshsource.com', '905-333-3333'),
('FreshSource Oakville', 'oakville@freshsource.com', '905-444-4444'),
('FreshSource Scarborough', 'scar@freshsource.com', '416-555-5555'),
('FreshSource Brampton', 'brampton@freshsource.com', '905-666-6666'),
('FreshSource Milton', 'milton@freshsource.com', '905-777-7777'),
('FreshSource Vaughan', 'vaughan@freshsource.com', '905-888-8888'),
('FreshSource Hamilton', 'hamilton@freshsource.com', '905-999-9999'),
('FreshSource Waterloo', 'waterloo@freshsource.com', '519-123-4567');
GO

-------------------------------------------------------------------------------
-- 2. SEED PRODUCTS (10 rows)
-------------------------------------------------------------------------------

INSERT INTO Products 
(Name, Category, UnitPrice, QuantityOnHand, ReorderLevel, ImageUrl, SupplierId)
VALUES
('Apple', 'Fruit', 1.99, 50, 10, 'https://example.com/apple.png', 1),
('Banana', 'Fruit', 0.79, 120, 20, 'https://example.com/banana.png', 2),
('Orange', 'Fruit', 2.49, 80, 15, 'https://example.com/orange.png', 3),
('Milk 1L', 'Dairy', 3.49, 60, 12, 'https://example.com/milk.png', 4),
('Eggs 12ct', 'Dairy', 4.99, 40, 8, 'https://example.com/eggs.png', 5),
('Bread Loaf', 'Bakery', 2.99, 70, 20, 'https://example.com/bread.png', 6),
('Chicken Breast', 'Meat', 8.99, 30, 10, 'https://example.com/chicken.png', 7),
('Pasta 500g', 'Pantry', 1.49, 90, 25, 'https://example.com/pasta.png', 8),
('Rice 5kg', 'Pantry', 9.99, 25, 5, 'https://example.com/rice.png', 9),
('Tomatoes', 'Vegetable', 3.29, 45, 12, 'https://example.com/tomatoes.png', 10);
GO

-------------------------------------------------------------------------------
-- 3. SEED PURCHASE ORDERS (10 rows)
-- Statuses: Pending, Received, Cancelled
-------------------------------------------------------------------------------

INSERT INTO PurchaseOrders
(ProductId, SupplierId, Quantity, Status, CreatedAt)
VALUES
(1, 1, 20, 'Pending', GETUTCDATE()),
(2, 2, 30, 'Pending', GETUTCDATE()),
(3, 3, 10, 'Received', GETUTCDATE()),
(4, 4, 15, 'Pending', GETUTCDATE()),
(5, 5, 12, 'Cancelled', GETUTCDATE()),
(6, 6, 25, 'Pending', GETUTCDATE()),
(7, 7, 18, 'Pending', GETUTCDATE()),
(8, 8, 22, 'Received', GETUTCDATE()),
(9, 9, 8,  'Pending', GETUTCDATE()),
(10, 10, 14, 'Pending', GETUTCDATE());
GO
