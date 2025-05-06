import React, { useState, useEffect } from 'react';
import InputSemBordaComLabel from '../molecules/InputSemBordaComLabel';
import BotaoPrimario from '../atoms/BotaoPrimario';
import {
    UserOutlined,
    MailOutlined,
    IdcardOutlined,
    PhoneOutlined,
    LockOutlined
} from '@ant-design/icons';

const FormEdicaoPerfil = ({ onSubmit, erros, initialData = {} }) => {
    const [formData, setFormData] = useState({
        nome: '',
        email: '',
        cpf: '',
        telefone: '',
        senha: '',
        confirmarSenha: ''
    });

    useEffect(() => {
        if (initialData) {
            setFormData({
                nome: initialData.nome || '',
                email: initialData.email || '',
                cpf: initialData.cpf || '',
                telefone: initialData.telefone || '',
                senha: '',
                confirmarSenha: ''
            });
        }
    }, [initialData]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({ ...prev, [name]: value }));
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        onSubmit(formData);
    };

    return (
        <form className="w-full" onSubmit={handleSubmit}>
            <InputSemBordaComLabel
                label="Nome"
                name="nome"
                value={formData.nome}
                onChange={handleChange}
                placeholder="Nome completo"
                icone={<UserOutlined className="text-green-dark" />}
                erro={erros.nome}
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
                label="CPF"
                name="cpf"
                value={formData.cpf}
                onChange={handleChange}
                placeholder="XXX.XXX.XXX-XX"
                icone={<IdcardOutlined className="text-green-dark" />}
                erro={erros.cpf}
                mask="999.999.999-99"
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

            <InputSemBordaComLabel
                label="Nova Senha (opcional)"
                name="senha"
                type="password"
                value={formData.senha}
                onChange={handleChange}
                placeholder="Deixe em branco para manter a atual"
                icone={<LockOutlined className="text-green-dark" />}
                erro={erros.senha}
            />

            <InputSemBordaComLabel
                label="Confirme a Nova Senha"
                name="confirmarSenha"
                type="password"
                value={formData.confirmarSenha}
                onChange={handleChange}
                placeholder="Repita a nova senha"
                icone={<LockOutlined className="text-green-dark" />}
                erro={erros.confirmarSenha}
            />

            <div className='w-1/2 mx-auto mt-8'>
                <BotaoPrimario texto="Salvar Alterações" type="submit" />
            </div>
        </form>
    );
};

export default FormEdicaoPerfil;
