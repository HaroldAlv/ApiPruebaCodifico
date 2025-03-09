-------------------CONSULTAS PARA PRUEBA-------------------
-----------------------------------------------------------
-------------------SALES DATE PREDICTION-------------------
CREATE VIEW vw_NextPredictedOrder as
WITH OrderIntervals AS (
    SELECT 
        o.custid,
        o.OrderDate,
        COALESCE(LAG(o.OrderDate) OVER (PARTITION BY o.custid ORDER BY o.OrderDate), o.OrderDate) AS PreviousOrderDate
    FROM sales.Orders o
),
AvgOrderGap AS (
    SELECT 
        custid,
        AVG(DATEDIFF(DAY, PreviousOrderDate, OrderDate)) AS AvgDaysBetweenOrders
    FROM OrderIntervals
    WHERE PreviousOrderDate IS NOT NULL
    GROUP BY custid
),
LastOrder AS (
    SELECT 
        o.custid,
        c.companyname,
        MAX(o.OrderDate) AS LastOrderDate
    FROM sales.Orders o
    JOIN sales.Customers c ON o.custid = c.custid
    GROUP BY o.custid, c.companyname
)
SELECT 
    l.companyname CustomerName,
    l.LastOrderDate LastOrderDate,
    DATEADD(DAY, a.AvgDaysBetweenOrders, l.LastOrderDate) AS NextPredictedOrder
FROM LastOrder l
LEFT JOIN AvgOrderGap a ON l.custid = a.custid;
-------------------GET CLIENT ORDERS-------------------
CREATE or alter VIEW GetClientOrders
as
SELECT
o.custid,
o.orderid,
o.requireddate,
o.shippeddate,
o.shipname,
o.shipaddress,
o.shipcity
from Sales.Orders o	
-------------------GET EMPLOYEES-------------------
CREATE VIEW GetEmployees
as
SELECT 
e.empid,
e.firstname + ' ' + e.lastname FullName
FROM HR.Employees e
-------------------GET SHIPPERS-------------------
CREATE VIEW GetShippers
as
select 
sh.shipperid,
sh.companyname
from Sales.Shippers sh
-------------------GET PRODUCTS-------------------
CREATE VIEW GetProducts
as
SELECT
pr.productid,
pr.productname
FROM Production.Products pr
-------------------
CREATE or alter PROCEDURE AddNewOrder
    @EmpID INT,
	@CustID INT,
    @ShipperID INT,
    @ShipName NVARCHAR(80),
    @ShipAddress NVARCHAR(120),
    @ShipCity NVARCHAR(30),
    @OrderDate DATETIME,
    @RequiredDate DATETIME,
    @ShippedDate DATETIME = NULL,
    @Freight MONEY,
    @ShipCountry NVARCHAR(30),
    @ProductID INT,
    @UnitPrice MONEY,
    @Qty INT,
    @Discount DECIMAL(5,2)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @NewOrderID INT;

    INSERT INTO sales.Orders (empid, custid,shipperid, shipname, shipaddress, shipcity, orderdate, requireddate, shippeddate, freight, shipcountry)
    VALUES (@EmpID, @CustID,@ShipperID, @ShipName, @ShipAddress, @ShipCity, @OrderDate, @RequiredDate, @ShippedDate, @Freight, @ShipCountry);

    SET @NewOrderID = SCOPE_IDENTITY();

    INSERT INTO sales.OrderDetails (orderid, productid, unitprice, qty, discount)
    VALUES (@NewOrderID, @ProductID, @UnitPrice, @Qty, @Discount);

    PRINT 'Nueva orden creada con OrderID: ' + CAST(@NewOrderID AS NVARCHAR);
END;
-------------------EJEMPLO:-------------------
--EXEC AddNewOrder 
--    @EmpID = 5,
--	@CustID = 85,
--    @ShipperID = 1,
--    @ShipName = 'Ship to 46-A',
--    @ShipAddress = 'Calle 123',
--    @ShipCity = 'Bogotá',
--    @OrderDate = '20250102',
--    @RequiredDate = '20250210',
--    @ShippedDate = '20250123',
--    @Freight = 50.75,
--    @ShipCountry = 'Colombia',
--    @ProductID = 5,
--    @UnitPrice = 21.35,
--    @Qty = 2,
--    @Discount = 0.05;
