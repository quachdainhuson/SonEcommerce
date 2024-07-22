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
    <p>Địa Chỉ: {{ model.diachi }}, {{ model.fulldiachi}}</p>
    <p>Số Điện Thoại: {{ model.sdt }}</p>
    <p>Email: {{ model.diachiemail }}</p>

    <h2>Chi Tiết Đơn Hàng:</h2>
    <table>
        <thead>
            <tr>
                <th>Tên Sản Phẩm</th>
                <th>Giá</th>
                <th>Số Lượng</th>
                <th>Tổng Tiền</th>
            </tr>
        </thead>
        <tbody>
            {{ for chitiet in model.chitiethoadon }}
            <tr>
                <td>{{ chitiet.name }}</td>
                <td>{{ chitiet.price }}</td>
                <td>{{ chitiet.quantity }}</td>
                <td>{{ chitiet.price * chitiet.quantity}}</td>
            </tr>
            {{ end }}
        </tbody>
    </table>
    <h2>Tổng Tiền Đơn Hàng: {{ model.tongtien }}</h2>
</body>
</html>
