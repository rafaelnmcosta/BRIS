import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import axios from 'axios';

const EditarPerfil = () => {
  const { id } = useParams();
  const [usuario, setUsuario] = useState(null);
  const [formData, setFormData] = useState({ nome: usuario.nome, email: usuario.email, tipo: usuario.tipo });

  const UsuarioTeste = { id: 1, nome: 'João', email: 'joao@email.com', tipo: '2'};

  useEffect(() => {
    /*
    axios.get(`/api/usuarios/${id}`)
      .then(response => {
        setUsuario(response.data);
        setFormData({ nome: response.data.nome, email: response.data.email, tipo: response.data.tipo });
      })
      .catch(error => console.error('Erro ao buscar Usuario:', error));
    */
    setUsuario(UsuarioTeste);
  }, [id]);

  const handleSubmit = (event) => {
    event.preventDefault();
    axios.put(`/api/usuarios/${id}`, formData)
      .then(response => {
        setUsuario(response.data);
        alert('Usuario atualizado com sucesso!');
      })
      .catch(error => console.error('Erro ao atualizar Usuario:', error));
  };

  const handleChange = (event) => {
    setFormData({ ...formData, [event.target.name]: event.target.value });
  };

  if (!usuario) {
    return <div>Carregando...</div>;
  }

  return (
    <div>
      <h1>Editar Usuario: {formData.nome}</h1>
      <form onSubmit={handleSubmit}>
      <label>
          Nome:
          <input type="text" name="nome" value={formData.nome} onChange={handleChange} />
        </label>
        <label>
          E-mail:
          <input type="text" name="email" value={formData.email} onChange={handleChange} />
        </label>
        <label>
          Tipo:
          <input type="text" name="tipo" value={formData.tipo} onChange={handleChange} />
        </label>
        <button type="submit">Salvar</button>
      </form>
    </div>
  );
};

export default EditarPerfil;
