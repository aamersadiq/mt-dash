import { createSlice } from '@reduxjs/toolkit';
import axiosInstance from '../../axiosInstance';

const accountSlice = createSlice({
  name: 'account',
  initialState: {
    accounts: [],
  },
  reducers: {
    setAccounts(state, action) {
      state.accounts = action.payload;
    },
  },
});

export const { setAccounts } = accountSlice.actions;

export const fetchAccounts = (userId) => async (dispatch) => {
  try {
    const response = await axiosInstance.get(`api/account/user/${userId}`);
    dispatch(setAccounts(response.data));
  } catch (error) {
    console.error('Fetching accounts failed', error);
  }
};

export const transferAmount = (fromAccountId, toAccountId, amount) => async (dispatch) => {
  try {
    await axiosInstance.post('api/account/transfer', {
      fromAccountId,
      toAccountId,
      amount,
    });
    // Refresh accounts after transfer
    dispatch(fetchAccounts(sessionStorage.getItem('userId')));
  } catch (error) {
    console.error('Transfer failed', error);
  }
};

export default accountSlice.reducer;
