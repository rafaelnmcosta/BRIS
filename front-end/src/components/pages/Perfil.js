import React, { useEffect, useState } from 'react';
import axios from 'axios';

const Perfil = () => {
  const id = localStorage.getItem("userId");
  console.log(id);
  const [usuario, setUsuario] = useState(null);
  useEffect(() => {
    axios.get(`/api/Usuarios/perfil/${id}`)
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

export default Perfil;
