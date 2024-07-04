<!DOCTYPE html>
<html>
<head>
    <style>
        body {
            font-family: Arial, sans-serif;
            line-height: 1.6;
        }
        h1, h2 {
            color: #333;
        }
        table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }
        th, td {
            border: 1px solid #ddd;
            padding: 8px;
            text-align: left;
        }
        th {
            background-color: #f2f2f2;
        }
    </style>
</head>
<body>
    <h1>{{ model.message }}</h1>
    <p>Tên Khách Hàng: {{ model.ten }}</p>
    <p>Địa Chỉ: {{ model.diachi }}</p>
    <p>Số Điện Thoại: {{ model.sdt }}</p>
    <p>Email: {{ model.diachiemail }}</p>

    <h2>Chi Tiết Đơn Hàng:</h2>
    <table>
        <thead>
            <tr>
                <th>Tên Sản Phẩm</th>
                <th>Giá</th>
                <th>Số Lượng</th>
                <!-- Nếu cần thêm thông tin khác của sản phẩm, bạn có thể thêm vào đây -->
            </tr>
        </thead>
        <tbody>
            {{ for chitiet in model.chitiethoadon }}
            <tr>
                <td>{{ chitiet.name }}</td>
                <td>{{ chitiet.price }}</td>
                <td>{{ chitiet.quantity }}</td>
                <!-- Nếu cần thêm thông tin khác của sản phẩm, bạn có thể thêm vào đây -->
            </tr>
            {{ end }}
        </tbody>
    </table>
</body>
</html>
