import { createSlice } from '@reduxjs/toolkit';
import { jwtDecode } from 'jwt-decode';
import axiosInstance from '../../axiosInstance';

const authSlice = createSlice({
  name: 'auth',
  initialState: {
    userId: null,
    token: null,
    roles: [],
  },
  reducers: {
    setUser(state, action) {
      state.userId = action.payload.userId;
      state.token = action.payload.token;
      state.roles = action.payload.roles;
      sessionStorage.setItem('token', action.payload.token);
      sessionStorage.setItem('userId', action.payload.userId);
      sessionStorage.setItem('roles', JSON.stringify(action.payload.roles));
    },
    logout(state) {
      state.userId = null;
      state.token = null;
      state.roles = [];
      sessionStorage.removeItem('token');
      sessionStorage.removeItem('userId');
      sessionStorage.removeItem('roles');
    },
  },
});

export const { setUser, logout } = authSlice.actions;

export const login = (username, password) => async (dispatch) => {
  try {
    const response = await axiosInstance.post('api/auth/login', {
      username,
      password,
    });
    const token = response.data.token;
    const decoded = jwtDecode(token);
    const userId = decoded.id;
    const roles = decoded.role;
    dispatch(setUser({ userId, token, roles }));
    return Promise.resolve(); // Return resolved promise
  } catch (error) {
    return Promise.reject(error); // Return rejected promise
  }
};

export default authSlice.reducer;
