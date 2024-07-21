import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import axios from 'axios';

const EditarAnimal = () => {
  const { id } = useParams();
  const [animal, setAnimal] = useState(null);
  const [formData, setFormData] = useState({ info: animal.info});

  useEffect(() => {
    axios.get(`/api/Animais/${id}`)
      .then(response => {
        setAnimal(response.data);
        setFormData({ nome: response.data.nome, idade: response.data.idade, raca: response.data.raca });
      })
      .catch(error => console.error('Erro ao buscar animal:', error));
  }, [id]);

  const handleSubmit = (event) => {
    event.preventDefault();
    axios.put(`/api/Animais/${id}`, formData)
      .then(response => {
        setAnimal(response.data);
        alert('Animal atualizado com sucesso!');
      })
      .catch(error => console.error('Erro ao atualizar animal:', error));
  };

  const handleChange = (event) => {
    setFormData({ ...formData, [event.target.name]: event.target.value });
  };

  if (!animal) {
    return <div>Carregando...</div>;
  }

  return (
    <div>
      <h1>Editar Animal {id}</h1>
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

export default EditarAnimal;
