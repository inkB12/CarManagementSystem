# Ứng Dụng Quản Lý Xe Điện Đại Lý (EV Dealer Management System - EV DMS)

[![C#](https://img.shields.io/badge/Language-C%23-239120?style=for-the-badge&logo=c-sharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![.NET MVC](https://img.shields.io/badge/Framework-.NET%20MVC-512BD4?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com/en-us/apps/aspnet)
[![Database](https://img.shields.io/badge/Database-SQL%20Server-CC2927?style=for-the-badge&logo=microsoft-sql-server)](https://www.microsoft.com/en-us/sql-server)

## ⚡ Giới Thiệu Dự Án Sơ Lược

**EV DMS** là một hệ thống quản lý tập trung, được phát triển để hỗ trợ các đại lý xe điện (Electric Vehicles - EV) tối ưu hóa các quy trình kinh doanh cốt lõi.

Ứng dụng được xây dựng trên nền tảng **ASP.NET Core MVC (C#)**, tuân thủ kiến trúc **Model-View-Controller**.

---

## 🔑 Tài Khoản Đăng Nhập Thử Nghiệm

Bạn có thể sử dụng các tài khoản dưới đây để đăng nhập và kiểm tra các chức năng của hệ thống. Đây là các tài khoản đã được gán **Role** và **Policy** khác nhau trong hệ thống **ASP.NET Core Identity**.

| Vai trò (Role) | Tên đăng nhập (Username) | Mật khẩu (Password) |
| :--- | :--- | :--- | :--- |
| **Quản trị viên** | `admin@gmail.com` | `123456` |
| **Nhân viên** | `staff@gmail.com` | `123456` |
| **Khách hàng 1** | `nthanhn2491@gmail.com` | `123456` |
| **Khách hàng 2** | `customer@evdealer.com` | `123456` |

---

## 🚀 Hướng Dẫn Khởi Chạy (Local Setup)

1.  **Clone Repository:**
    ```bash
    git clone [Địa chỉ Repository của bạn]
    cd EV_DMS
    ```
2.  **Cấu hình Database:**
    * Mở file `appsettings.json` và cập nhật chuỗi kết nối (`ConnectionString`) để trỏ đến SQL Server cục bộ của bạn.
3.  **Chạy Ứng dụng:**
    * Mở giải pháp (.sln) bằng Visual Studio và nhấn **F5**, hoặc chạy lệnh:
        ```bash
        dotnet run
        ```
4.  Truy cập vào địa chỉ hiển thị (thường là `https://localhost:7062/`).
