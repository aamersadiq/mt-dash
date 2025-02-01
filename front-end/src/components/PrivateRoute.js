import React from 'react';
import { Route, Navigate } from 'react-router-dom';
import { useSelector } from 'react-redux';

function PrivateRoute({ element: Component, requiredRole, ...rest }) {
  const token = useSelector((state) => state.auth.token);
  const roles = useSelector((state) => state.auth.roles);

  const hasRequiredRole = roles.includes(requiredRole);

  return (
    <Route
      {...rest}
      element={token && hasRequiredRole ? <Component /> : <Navigate to="/login" />}
    />
  );
}

export default PrivateRoute;
