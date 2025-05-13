import React, { useState, useEffect } from 'react';
import InputSemBordaComLabel from '../molecules/InputSemBordaComLabel';
import BotaoPrimario from '../atoms/BotaoPrimario';
import { useValidation } from '../../services/ValidationContext';
import {
    UserOutlined,
    MailOutlined,
    IdcardOutlined,
    PhoneOutlined,
    LockOutlined
} from '@ant-design/icons';

const FormEdicaoPerfil = ({ onSubmit, erros, initialData = {} }) => {
    const [errors, setErrors] = React.useState({});
    const [formData, setFormData] = useState({
        nome: '',
        email: '',
        cpf: '',
        telefone: '',
        senha: '',
        confirmarSenha: ''
    });

    const {
        validarCampoObrigatorio,
        validarEmail,
        validarCPF,
        validarTelefone,
        validarSenha,
        validarConfirmacaoSenha,
    } = useValidation();

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
        let { name, value } = e.target;

        if (['cpf', 'telefone'].includes(name)) {
            value = value.replace(/\D/g, '');
        }

        setFormData((prev) => ({ ...prev, [name]: value }));

        // Valida apenas o campo alterado
        let erro = '';
        switch (name) {
            case 'nome':
                erro = validarCampoObrigatorio(value);
                break;
            case 'email':
                erro = validarCampoObrigatorio(value) || validarEmail(value);
                break;
            case 'cpf':
                erro = validarCampoObrigatorio(value) || validarCPF(value);
                break;
            case 'telefone':
                erro = validarTelefone(value);
                break;
            case 'senha':
                erro = validarCampoObrigatorio(value) || validarSenha(value);
                break;
            case 'confirmarSenha':
                erro = validarConfirmacaoSenha(formData.senha, value);
                break;
            default:
                break;
        }

        setErrors((prevErros) => ({ ...prevErros, [name]: erro }));
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
                type="passwordCadastro"
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
