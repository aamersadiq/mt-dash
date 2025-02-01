import React, { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { transferAmount } from '../../store/account/accountSlice';
import { useNavigate } from 'react-router-dom';

function Transfer() {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const accounts = useSelector((state) => state.account.accounts);
  const [fromAccountId, setFromAccountId] = useState('');
  const [toAccountId, setToAccountId] = useState('');
  const [amount, setAmount] = useState('');
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  const handleTransfer = async (e) => {
    e.preventDefault();
    if (!fromAccountId) {
      setError('Please select a From Account.');
      return;
    }
    if (!toAccountId) {
      setError('Please select a To Account.');
      return;
    }
    if (fromAccountId === toAccountId) {
      setError('From Account and To Account cannot be the same.');
      return;
    }
    if (!amount || parseFloat(amount) <= 0) {
      setError('Please enter a valid amount greater than 0.');
      return;
    }
    setError('');
    try {
      await dispatch(transferAmount(fromAccountId, toAccountId, parseFloat(amount)));
      setSuccess('Transfer successful.');
      setFromAccountId('');
      setToAccountId('');
      setAmount('');
    } catch (err) {
      setError('Transfer failed. Please try again.');
    }
  };

  return (
    <div className="container mt-4">
      <h2>Transfer Between Accounts</h2>
      {error && <div className="alert alert-danger mt-3">{error}</div>}
      {success && <div className="alert alert-success mt-3">{success}</div>}
      <form onSubmit={handleTransfer} className="mt-4">
        <div className="form-group">
          <label>From Account</label>
          <select
            className="form-control"
            value={fromAccountId}
            onChange={(e) => setFromAccountId(e.target.value)}
          >
            <option value="">Select Account</option>
            {accounts.map((account) => (
              <option key={account.id} value={account.id}>
                Account #{account.id}
              </option>
            ))}
          </select>
        </div>
        <div className="form-group">
          <label>To Account</label>
          <select
            className="form-control"
            value={toAccountId}
            onChange={(e) => setToAccountId(e.target.value)}
          >
            <option value="">Select Account</option>
            {accounts.map((account) => (
              <option key={account.id} value={account.id}>
                Account #{account.id}
              </option>
            ))}
          </select>
        </div>
        <div className="form-group">
          <label>Amount</label>
          <input
            type="number"
            className="form-control"
            value={amount}
            onChange={(e) => setAmount(e.target.value)}
          />
        </div>
        <button type="submit" className="btn btn-primary mt-3">Transfer</button>
      </form>
      <button className="btn btn-secondary mt-3" onClick={() => navigate('/dashboard')}>
        Back to Dashboard
      </button>
    </div>
  );
}

export default Transfer;
