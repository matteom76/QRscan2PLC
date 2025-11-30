# QRscan2PLC

QRscan2PLC is a mobile application that allows you to connect your smartphone or tablet to Siemens PLC devices or to an MQTT broker and interact with them through customizable HMI-like interface screens.  
Each interface screen can be associated with a QR code or barcode, enabling immediate access simply by scanning the corresponding code.

---

## 📌 Overview

QRscan2PLC provides a flexible way for operators and technicians to read, write, and publish data through PLCs or MQTT using dynamically generated screens.  
QR/barcodes act as keys that automatically load the correct interface based on their structure (length or specific content).

---

## 🚀 Key Features

### 🔗 Wi-Fi Connectivity
- Connect the mobile device to:
  - Siemens PLCs (S7-300/400, S7-1200/1500)  
  - Any MQTT broker via TCP  

### 🧩 QR/Barcode Type Definition
- Define different types of QR codes and barcodes based on:
  - Code length  
  - Presence of specific strings  

### 🎛 Custom HMI-like Screens
Create interface screens containing objects capable of:
- Reading and writing PLC data  
- Subscribing to MQTT topics  
- Publishing data to MQTT clients  

### 🎯 Screen Assignment
Link each interface screen to a specific QR/barcode type.

### 📲 Automatic Screen Loading
Upon scanning a corresponding QR/barcode:
- The associated interface screen is displayed  
- Scanned data can be:
  - Written directly into PLC memory  
  - Published to the MQTT broker  

### 📡 Code Reader Support
- The built-in reader can scan various QR codes and barcodes.

---

## 🏭 Supported Devices

### ✔ PLC Models
- Siemens S7-300  
- Siemens S7-400  
- Siemens S7-1200  
- Siemens S7-1500  

### ✔ MQTT
- Any MQTT broker reachable via TCP

---

## 📝 Notes
QRscan2PLC is designed for flexibility in industrial environments, enabling rapid visualization and configuration of PLC/MQTT data through scan-based access.

---

## 📧 Contact
For information, support, or feedback, feel free to reach out to the project maintainer.

