import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import axios from 'axios';

const Animal = () => {
  const { id } = useParams();
  const [animal, setAnimal] = useState(null);

  useEffect(() => {
    axios.get(`http://localhost:5206/api/Animais/${id}`)
      .then(response => setAnimal(response.data))
      .catch(error => console.error('Erro ao buscar animal:', error));
  }, [id]);

  if (!animal) {
    return <div>Carregando...</div>;
  }

  return (
    <div>
      <h1>Informações do Animal de cadastro: {id}</h1>
      <p>info: {animal.info}</p>
    </div>
  );
};

export default Animal;
