SELECT "UserId", "Email", "UserName", "Password", "UserType"
FROM public."Users";

SELECT "CustomerId", "Name", "Email", "UserId"
FROM public."Customers";

SELECT "ProductId", "Name", "Price", "Quantity"
FROM public."Products";

SELECT "OrderId", "OrderDate", "Status", "CustomerId", "ProductId", "Quantity", "Price", "ZipCode", "DeliveryAddress", "NumberAddress"
FROM public."Orders";