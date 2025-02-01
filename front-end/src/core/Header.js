import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Link, useNavigate } from 'react-router-dom';
import { logout } from '../store/auth/authSlice';
import 'bootstrap/dist/css/bootstrap.min.css';
import './Header.css'; // Import the CSS file

function Header() {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const isLoggedIn = useSelector((state) => state.auth.isLoggedIn); // Assuming you have an isLoggedIn flag

  const handleLogout = () => {
    dispatch(logout());
    navigate('/login');
  };

  return (
    <header className="navbar navbar-expand-lg navbar-light bg-light">
      <div className="container">
        <Link className="navbar-brand" to="/">My Bank</Link>
        <button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
          <span className="navbar-toggler-icon"></span>
        </button>
        <div className="collapse navbar-collapse" id="navbarNav">
          <ul className="navbar-nav ml-auto align-right">
            {isLoggedIn && (
              <li className="nav-item">
                <button className="btn btn-danger" onClick={handleLogout}>Log Out</button>
              </li>
            )}
          </ul>
        </div>
      </div>
    </header>
  );
}

export default Header;
