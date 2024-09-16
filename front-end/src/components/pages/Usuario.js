import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import axios from 'axios';

const Usuario = () => {
  const { id } = useParams();
  const [usuario, setUsuario] = useState(null);

  useEffect(() => {
    axios.get(`http://localhost:5206/api/usuarios/${id}`)
      .then(response => setUsuario(response.data))
      .catch(error => console.error('Erro ao buscar Usuario:', error));
  }, [id]);

  if (!usuario) {
    return <div>Carregando...</div>;
  }

  return (
    <div>
      <h1>Informações do Usuario de cadastro: {id}</h1>
      <p>Nome: {usuario.nome}</p>
      <p>E-mail: {usuario.email}</p>
      <p>Tipo: {usuario.tipo}</p>
    </div>
  );
};

export default Usuario;
