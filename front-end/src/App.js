import React from 'react';
import { Route, Routes, Navigate } from 'react-router-dom';
import { useSelector } from 'react-redux';
import Login from './components/Login';
import AccountDashboard from './components/AccountDashboard';
import Transfer from './components/Transfer';
import PrivateRoute from './components/PrivateRoute';

function App() {
  const token = useSelector((state) => state.auth.token);

  return (
    <div>
      <Routes>
        <Route path="/" element={<Navigate to="/login" />} />
        <Route path="/login" element={<Login />} />
        <Route path="/dashboard" element={token ? <AccountDashboard /> : <Navigate to="/login" />} />
        <Route path="/transfer" element={token ? <Transfer /> : <Navigate to="/login" />} />
        <Route path="*" element={<Navigate to="/login" />} />
      </Routes>
    </div>
  );
}

export default App;
