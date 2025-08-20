import React, { useState, useEffect } from 'react';
import InputSemBordaComLabel from '../molecules/InputSemBordaComLabel';
import BotaoPrimario from '../atoms/BotaoPrimario';
import { IdcardOutlined, UserOutlined, MailOutlined, PhoneOutlined } from '@ant-design/icons';

const FormEdicaoGranja = ({ onSubmit, erros = {}, initialData = {} }) => {
  const [formData, setFormData] = useState({
    nomePropriedade: '',
    cnpj: '',
    email: '',
    telefone: ''
  });

  useEffect(() => {
    if (initialData) {
      setFormData({
        nomePropriedade: initialData.nomePropriedade || '',
        cnpj: initialData.cnpj || '',
        email: initialData.email || '',
        telefone: initialData.telefone || ''
      });
    }
  }, [initialData]);

  const handleChange = (e) => {
    let { name, value } = e.target;

    // Remove máscaras de campos específicos
    if (['cnpj', 'telefone'].includes(name)) {
      value = value.replace(/\D/g, '');
    }

    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    onSubmit({ ...formData, id: initialData.id });
  };

  return (
    <form className="w-full" onSubmit={handleSubmit}>
      <InputSemBordaComLabel
        label="Nome da Granja"
        name="nomePropriedade"
        value={formData.nomePropriedade}
        onChange={handleChange}
        placeholder="Nome da propriedade"
        icone={<UserOutlined className="text-green-dark" />}
        erro={erros.nomePropriedade}
      />

      <InputSemBordaComLabel
        label="CNPJ"
        name="cnpj"
        value={formData.cnpj}
        onChange={handleChange}
        placeholder="XX.XXX.XXX/XXXX-XX"
        icone={<IdcardOutlined className="text-green-dark" />}
        erro={erros.cnpj}
        mask="99.999.999/9999-99"
      />

      <InputSemBordaComLabel
        label="E-mail"
        name="email"
        type="email"
        value={formData.email}
        onChange={handleChange}
        placeholder="exemplo@email.com"
        icone={<MailOutlined className="text-green-dark" />}
        erro={erros.email}
      />

      <InputSemBordaComLabel
        label="Telefone"
        name="telefone"
        type="tel"
        value={formData.telefone}
        onChange={handleChange}
        placeholder="(XX) XXXXX-XXXX"
        icone={<PhoneOutlined className="text-green-dark" />}
        erro={erros.telefone}
        mask="(99) 99999-9999"
      />

      <div className="w-1/2 mx-auto mt-8">
        <BotaoPrimario texto="Salvar Alterações" type="submit" />
      </div>
    </form>
  );
};

export default FormEdicaoGranja;
