# FZ Academy Sınaq İmtahanı

This repository contains a fully functional, self-contained web form for the FZ Academy Exam (28.12.2025).

## 🎨 Features

- **Full Graphics**: Beautiful gradient design with professional styling
- **Responsive Design**: Works perfectly on desktop, tablet, and mobile devices
- **Interactive Form**: Includes form validation and interactive elements
- **Self-Contained**: All CSS and JavaScript embedded - no external dependencies
- **Accessible**: Proper semantic HTML and ARIA labels

## 📋 Form Fields

1. **Ad, Soyad** (Name, Surname) - Required
2. **Qrup** (Group) - Required  
3. **Əlaqə nömrəsi** (Contact Number) - Required

## 🚀 How to Use

### Option 1: Open Directly
Simply open `index.html` in any modern web browser:
```bash
open index.html
```

### Option 2: Use a Local Server
For the best experience, serve the file using a local HTTP server:

```bash
# Using Python 3
python3 -m http.server 8000

# Using Python 2
python -m SimpleHTTPServer 8000

# Using Node.js (http-server)
npx http-server
```

Then open http://localhost:8000/index.html in your browser.

## 🎯 Interactive Features

- **Form Validation**: Required fields are validated before submission
- **Clear Form**: Reset all fields with one click
- **Help Button**: Fixed help button in the bottom-right corner
- **Visual Feedback**: Input fields highlight on focus
- **Mobile Friendly**: Fully responsive layout

## 🖼️ Design Elements

- **Color Scheme**: Purple gradient background with tan/beige form header
- **Typography**: Clean, professional fonts with proper hierarchy
- **Buttons**: Material Design-inspired with hover effects
- **Icons**: Lock icon for privacy, Google Forms branding

## 📱 Browser Compatibility

- ✅ Chrome/Edge (Latest)
- ✅ Firefox (Latest)
- ✅ Safari (Latest)
- ✅ Mobile Browsers

## 📝 Source

The form design is based on Google Forms styling, converted to a standalone HTML page with embedded CSS and JavaScript for offline functionality.

---

**Note**: This is a demo form. The "Next" button shows an alert instead of actually submitting data.
