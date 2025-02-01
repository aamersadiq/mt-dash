import React from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import { useSelector } from 'react-redux';
import Header from './core/Header';
import Footer from './core/Footer';
import Login from './components/login/Login';
import AccountDashboard from './components/dashboard/AccountDashboard';
import Transfer from './components/transfer/Transfer';
import 'bootstrap/dist/css/bootstrap.min.css';

function App() {
  const token = useSelector((state) => state.auth.token);

  return (
    <div className="d-flex flex-column min-vh-100">
      <Header />
      <main role="main" className="flex-grow-1">
        <div className="container mt-4">
          <Routes>
            <Route path="/" element={<Navigate to="/login" />} />
            <Route path="/login" element={<Login />} />
            <Route path="/dashboard" element={token ? <AccountDashboard /> : <Navigate to="/login" />} />
            <Route path="/transfer" element={token ? <Transfer /> : <Navigate to="/login" />} />
            <Route path="*" element={<Navigate to="/login" />} />
          </Routes>
        </div>
      </main>
      <Footer />
    </div>
  );
}

export default App;
