<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>OTP Email</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }
        .container {
            width: 100%;
            max-width: 600px;
            margin: 0 auto;
            padding: 20px;
            background-color: #ffffff;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        }
        .header {
            text-align: center;
            padding: 10px;
            background-color: #4CAF50;
            color: #ffffff;
        }
        .content {
            padding: 20px;
            text-align: center;
        }
        .footer {
            text-align: center;
            padding: 10px;
            background-color: #f4f4f4;
            color: #888888;
            font-size: 12px;
        }
        .otp {
            font-size: 24px;
            font-weight: bold;
            color: #333333;
        }
    </style>
</head>
<body>
    <div class="container">
        <div class="header">
            <h1>Đây là mã xác thực email của bạn. Vui lòng không chia sẻ cho bất kì ai!!</h1>
        </div>
        <div class="content">
            <h1>{{model.message}}</h1>
        </div>
        <div class="footer">
            <h4>Xin trân thành cảm ơn!</h4>
        </div>
    </div>
</body>
</html>
