import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import axios from 'axios';

const Perfil = () => {
  const { id } = useParams();
  const [usuario, setUsuario] = useState(null);

  const UsuarioTeste = { id: 1, nome: 'João', email: 'joao@email.com', tipo: '2'};

  useEffect(() => {
    /*
    axios.get(`/api/usuarios/${id}`)
      .then(response => setUsuario(response.data))
      .catch(error => console.error('Erro ao buscar Usuario:', error));
    */
   setUsuario(UsuarioTeste);
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
