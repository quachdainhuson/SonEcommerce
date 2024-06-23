@model dynamic

<html>
<body>
    <h1>{{model.message}}</h1>
    <p>Chi tiết đơn hàng:</p>
    <ul>
        <li>Email đặt hàng: {{orderEmail.CustomerAddress}}</li>
        
        <!-- Thêm thông tin khác từ đối tượng order -->
    </ul>
</body>
</html>