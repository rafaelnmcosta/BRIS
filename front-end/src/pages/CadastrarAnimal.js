import React, { useState } from 'react';
import axios from 'axios';

const CadastrarAnimal = () => {
  const [formData, setFormData] = useState({ info: '' });

  const handleSubmit = (event) => {
    event.preventDefault();
    axios.post('http://localhost:5206/api/Animais', formData)
      .then(response => {
        alert('Animal cadastrado com sucesso!');
        setFormData({ info: '' }); // Limpa o formulário após o cadastro
      })
      .catch(error => {
        console.error('Erro ao cadastrar animal:', error);
        alert('Erro ao cadastrar animal!');
      });
        
  };

  const handleChange = (event) => {
    setFormData({ ...formData, [event.target.name]: event.target.value });
  };

  return (
    <div>
      <h1>Cadastrar Novo Animal</h1>
      <form onSubmit={handleSubmit}>
        <label>
          Info:
          <input type="text" name="info" value={formData.info} onChange={handleChange} />
        </label>
        <button type="submit">Salvar</button>
      </form>
    </div>
  );
};

export default CadastrarAnimal;
