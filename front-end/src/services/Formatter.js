import React from 'react';
import { Typography } from 'antd';
import dayjs from 'dayjs';

/**
 * Formata uma data para o padrão DD/MM/YYYY às HH:mm.
 * Retorna um componente 'Nunca' se a data for nula.
 * @param {string | Date} data - A data a ser formatada.
 * @returns {React.Component | string}
 */
export const formatarData = (data) => {
    if (!data) return <Typography.Text type="secondary">Nunca</Typography.Text>;
    return dayjs(data).format('DD/MM/YYYY [às] HH:mm');
};

/**
 * Formata um CPF (12345678900 -> 123.456.789-00).
 * @param {string} cpf - String contendo 11 dígitos.
 * @returns {string} O CPF formatado.
 */
export const formatarCPF = (cpf) => {
    if (!cpf || cpf.length !== 11) return cpf;
    return cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4');
};

/**
 * Formata um CNPJ (12345678000199 -> 12.345.678/0001-99).
 * @param {string} cnpj - String contendo 14 dígitos.
 * @returns {string} O CNPJ formatado.
 */
export const formatarCNPJ = (cnpj) => {
    if (!cnpj || cnpj.length !== 14) return cnpj;
    return cnpj.replace(/(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/, '$1.$2.$3/$4-$5');
};

/**
 * Formata um telefone (ex: 62999999999 -> (62) 99999-9999).
 * @param {string} tel - String contendo 10 ou 11 dígitos.
 * @returns {string} O telefone formatado.
 */
export const formatarTelefone = (tel) => {
    if (!tel || tel.length < 10) return tel;
    if (tel.length === 11) {
        return tel.replace(/(\d{2})(\d{5})(\d{4})/, '($1) $2-$3');
    }
    return tel.replace(/(\d{2})(\d{4})(\d{4})/, '($1) $2-$3');
};

/**
 * Formata um CEP (ex: 74000000 -> 74000-000).
 * @param {string} cep - String contendo 8 dígitos.
 * @returns {string} O CEP formatado.
 */
export const formatarCEP = (cep) => {
    if (!cep || cep.length !== 8) return cep;
    return cep.replace(/(\d{5})(\d{3})/, '$1-$2');
};
