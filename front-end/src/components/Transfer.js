import React, { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { transferAmount } from '../features/account/accountSlice';
import { useNavigate } from 'react-router-dom';

function Transfer() {
  const dispatch = useDispatch();
  const navigate = useNavigate(); 
  const accounts = useSelector((state) => state.account.accounts);
  const [fromAccountId, setFromAccountId] = useState('');
  const [toAccountId, setToAccountId] = useState('');
  const [amount, setAmount] = useState('');

  const handleTransfer = (e) => {
    e.preventDefault();
    dispatch(transferAmount(fromAccountId, toAccountId, parseFloat(amount)));
  };

  return (
    <div className="container">
      <h2>Transfer Between Accounts</h2>
      <form onSubmit={handleTransfer}>
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
