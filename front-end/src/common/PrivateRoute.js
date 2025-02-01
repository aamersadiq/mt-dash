import React from 'react';
import { useSelector } from 'react-redux';
import { Navigate } from 'react-router-dom';

const PrivateRoute = ({ element: Component, roles }) => {
  const token = useSelector((state) => state.auth.token);
  const userRoles = useSelector((state) => state.auth.roles);

  if (!token) {
    return <Navigate to="/login" />;
  }

  if (roles && roles.some((role) => userRoles.includes(role))) {
    return <Component />;
  }

  return <Navigate to="/dashboard" />;
};

export default PrivateRoute;
