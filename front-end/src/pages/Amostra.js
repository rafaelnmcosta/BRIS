import React, { useState } from 'react';
import { useParams } from 'react-router-dom';
import axios from 'axios';

const Amostra = () => {
  const { id } = useParams();
  const [formData, setFormData] = useState({ dose: '', data: '' });

  const handleSubmit = (event) => {
    event.preventDefault();
    axios.post(`/api/animais/${id}/nova-dose`, formData)
      .then(response => {
        alert('Nova dose cadastrada com sucesso!');
      })
      .catch(error => console.error('Erro ao cadastrar nova dose:', error));
  };

  const handleChange = (event) => {
    setFormData({ ...formData, [event.target.name]: event.target.value });
  };

  return (
    <div>
      <h1>Nova Dose para o Animal {id}</h1>
      <form onSubmit={handleSubmit}>
        <label>
          Dose:
          <input type="text" name="dose" value={formData.dose} onChange={handleChange} />
        </label>
        <label>
          Data:
          <input type="text" name="data" value={formData.data} onChange={handleChange} />
        </label>
        <button type="submit">Cadastrar</button>
      </form>
    </div>
  );
};

export default Amostra;
