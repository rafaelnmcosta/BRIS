import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import axios from 'axios';

const Animal = () => {
  const { id } = useParams();
  const [animal, setAnimal] = useState(null);

  const AnimalTeste = { id: 1, info: 'Bobby'};

  useEffect(() => {
    /*
    axios.get(`/api/animais/${id}`)
      .then(response => setAnimal(response.data))
      .catch(error => console.error('Erro ao buscar animal:', error));
    */
   setAnimal(AnimalTeste);
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
